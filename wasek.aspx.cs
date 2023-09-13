using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Resources;
using Prem.PTC;
using Prem.PTC.Memberships;
using Prem.PTC.Members;
using Prem.PTC.Payments;
using Titan.Pages;
using Titan.Cryptocurrencies;
using System.Text;
using Titan.Crowdfunding;

public partial class About : System.Web.UI.Page
{
    private delegate void DelMethodWithoutParam();
    bool HideCashBalanceDepositCommissionColumn;
    Dictionary<string, bool> autopayHelpArray;
    List<string> hiddenProperties;
    Member User;

    public int? BasePackId
    {
        get
        {
            return User.BaseLifetimePackId;
        }
    }

    public bool OnlyOneMembership
    {
        get
        {
            var MembershipsCache = new AvailableMembershipsCountCache();
            return (int)MembershipsCache.Get() == 1;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        AccessManager.RedirectIfDisabled(AppSettings.TitanFeatures.UpgradeEnabled);

        User = Member.CurrentInCache;

        if (!Page.IsPostBack)
        {
            //Bind the data dor DDL
            BindDataToDDL();
        }

        var delNoParam = new DelMethodWithoutParam(UpgradeMembership);
        TargetBalanceRadioButtonList.PageMethodWithNoParamRef = delNoParam;
        TargetBalanceRadioButtonList.JsFunction = "askForConfirmation(this); e.preventDefault();";

        var delNoParamPacks = new DelMethodWithoutParam(PurchaseBasePack);
        TargetBalanceBasePack.PageMethodWithNoParamRef = delNoParamPacks;
        TargetBalanceBasePack.JsFunction = "askForBasePackConfirmation(this); e.preventDefault();";

        ddlOptions.Attributes.Add("onchange", "updatePrice();");
        BasePacksDDL.Attributes.Add("onchange", "updateBasePackPrice();");
        autopayHelpArray = new Dictionary<string, bool>();

        HideCashBalanceDepositCommissionColumn = AreAllCashBalanceDepositCommissionsZero();

        Label10.Text = AppSettings.Memberships.TenPointsValue.ToClearString();
        LabelIle.Text = AppSettings.Memberships.UpgradePointsDiscount.ToString();

        //Display warning
        string WarningMessage = UpgradePageHelper.GetWarningMessage(User);
        WarningPanel.Visible = !String.IsNullOrEmpty(WarningMessage);
        WarningLiteral.Text = WarningMessage;

        if (AppSettings.Points.LevelMembershipPolicyEnabled)
            BuyUpgradePlaceHolder.Visible = false;

        hiddenProperties = MembershipProperty.GetPropsToHideForClient();
        TypesMembershipProperties.Text = AdPackTypeMembershipMapper.Mapper.GetHtmlFromCache();
        AdPackPropsPlaceHolder.Visible = AppSettings.TitanFeatures.AdvertAdPacksEnabled;

        var title = L1.UPGRADE;
        var desctiption = L1.UPGRADEINFO;

        if (OnlyOneMembership)
        {
            title = L1.MEMBERSHIP;
            desctiption = U6014.MEMBERSHIPINFO;
            BuyUpgradeAllPlaceHolder.Visible = false;
            MembershipDDL.Visible = false;
        }

        if (ddlOptions.Items.Count == 0)
            WarningPanel.Visible = BuyUpgradeAllPlaceHolder.Visible = false;

        TitleLiteral.Text = AppSettings.Points.LevelMembershipPolicyEnabled ? U5007.LEVELS : title;
        DescriptionLiteral.Text = AppSettings.Points.LevelMembershipPolicyEnabled ? String.Format(U5007.LEVELSINFO, AppSettings.PointsName) : desctiption;

        if (TitanFeatures.IsSimon)
        {
            SimonPlaceHolder.Visible = true;
            MembershipsPlaceHolder.Visible = false;
        }

        if (TitanFeatures.IsIrishpearse)
            MembershipsPlaceHolder.Visible = false;

        if (!AppSettings.TitanFeatures.ReferralsDirectEnabled && !AppSettings.TitanFeatures.ReferralsIndirectEnabled)
            CommissionsPlaceHolder.Visible = false;


        if (TitanFeatures.IsRollyhoo)
        {
            //investor can see only base packs, or member without purchased pack yet
            if (User.RollyhooCustomRoles == RollyhooCustomRoles.Investor || (Request.QueryString["bp"] != null && Request.QueryString["bp"] == "1") ||
                (User.RollyhooCustomRoles == RollyhooCustomRoles.Member && User.MembershipId == Membership.StandardMembershipId && User.BaseLifetimePackId == null))
            {
                MenuMultiView.ActiveViewIndex = 1;
                if (User.RollyhooCustomRoles == RollyhooCustomRoles.Member && User.BaseLifetimePackId == null)
                {
                    ErrorMessagePanel.Visible = true;
                    ErrorMessage.Text = string.Format("You have to purchase any pack to use {0}", AppSettings.Site.Name);
                }
            }
            else
            {
                MenuButtonsPlaceHolder.Visible = true;
                MenuButtonUpgrade.Text = L1.UPGRADE;
                MenuButtonBasePack.Text = "Base Pack";
            }

            if (!Page.IsPostBack)
                BindPackDataToDDL();

            if (BasePacksDDL.Items.Count == 0)
                PurchaseBasePackPlaceHolder.Visible = false;
        }
    }

    public void MenuButton_Click(object sender, EventArgs e)
    {
        ErrorMessagePanel.Visible = false;
        SuccMessagePanel.Visible = false;

        var TheButton = (Button)sender;
        var viewIndex = int.Parse(TheButton.CommandArgument);

        MenuMultiView.ActiveViewIndex = viewIndex;

        //Change button style
        foreach (Button b in MenuButtonPlaceHolder.Controls)
            b.CssClass = "";

        TheButton.CssClass = "ViewSelected";
    }

    protected void GenerateHiddenValues()
    {
        var sb = new StringBuilder();
        foreach (var wallet in Wallet.GetActive())
        {
            var exchange = ExchangeRates.Get(AppSettings.Site.CurrencyCode, wallet.Code);
            sb.Append(string.Format("<b id='{0}' style='display:none'>{1}</b>", string.Format("HiddenVal_{0}", wallet.Code), exchange.ToString()));
        }
        //WalletsHelperLiteral.Text = sb.ToString();
    }

    private void BindDataToDDL()
    {
        Money FirstElementPrice = Money.Zero;

        ddlOptions.Items.Clear();
        ddlOptions.Items.AddRange(UpgradePageHelper.GetMembershipPacks(User));
        ddlOptions.DataBind();

        PriceLiteral.Text = FirstElementPrice.ToString();
    }

    protected void ddlOptions_PreRender(object sender, EventArgs e)
    {
        for (int i = 0; i < ddlOptions.Items.Count; i++)
        {
            var item = ddlOptions.Items[i];
            item.Attributes.Add("data-price", new MembershipPack(Convert.ToInt32(item.Value)).GetPrice(User).ToMulticurrency().ToClearString());
        }
    }

    protected void BasePacksDDL_PreRender(object sender, EventArgs e)
    {
        for (int i = 0; i < BasePacksDDL.Items.Count; i++)
        {
            var item = BasePacksDDL.Items[i];
            item.Attributes.Add("data-price", new BaseLifetimePack(Convert.ToInt32(item.Value)).GetPrice(User).ToMulticurrency().ToClearString());
        }
    }

    private void BindPackDataToDDL()
    {
        var FirstElementPrice = Money.Zero;
        var packs = BaseLifetimePack.GetActivePacks();
        var projects = CrowdfundingProject.GetAllActiveProjects();
        var currentUserPack = User.BaseLifetimePackId != null ? new BaseLifetimePack((int)User.BaseLifetimePackId) : null;

        BasePacksDDL.Items.Clear();
        ProjectsDDL.Items.Clear();

        foreach (var pack in packs)        
            if (currentUserPack == null || (currentUserPack != null && currentUserPack.PackPrice < pack.PackPrice))
                BasePacksDDL.Items.Add(new ListItem(pack.Name, pack.Id.ToString()));        

        foreach (var project in projects)
            ProjectsDDL.Items.Add(new ListItem(project.Title, project.Id.ToString()));

        PackPriceLabel.Text = FirstElementPrice.ToString();
    }

    protected void UpgradeGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Create an instance of the datarow
            var rowData = e.Row.Cells[1].Text;

            //Set proper width
            //e.Row.Cells[0].ControlStyle.Width = Unit.Pixel(230);
            foreach (TableCell tc in e.Row.Cells)
                if (tc != e.Row.Cells[0])
                {
                    //tc.ControlStyle.Width = Unit.Pixel(50);
                    tc.ControlStyle.CssClass = "text-center";
                }


            if (hiddenProperties.Any(p => p == e.Row.Cells[0].Text))
                e.Row.CssClass = "displaynone";

            //Add image to index 1 (name)
            if (e.Row.RowIndex == 1)
            {
                int indexOfUpgrade = 0;
                foreach (TableCell tc in e.Row.Cells)
                    if (tc != e.Row.Cells[0])
                    {
                        string upgradeName = tc.Text;
                        string imageName = "standardbox";
                        if (upgradeName != Membership.Standard.Name)
                        {
                            //Because max premiumimagename is 7
                            //I know it is shitty work
                            imageName = (indexOfUpgrade > 7) ? "premiumbox7" : "premiumbox" + indexOfUpgrade.ToString();
                        }
                        tc.Text = "<div class=\"text-center\"><p><strong>" + upgradeName + "</strong></p><img src=\"Images/OneSite/Upgrade/" + imageName + ".png\" /></div>";
                        indexOfUpgrade++;
                    }
            }

            e.Row.Cells[0].Text = MembershipProperty.GetResourceLabel(e.Row.Cells[0].Text);

            foreach (TableCell tc in e.Row.Cells)
                if (tc != e.Row.Cells[0])
                    tc.Text = MembershipProperty.Format(e.Row.RowIndex, tc.Text);

            //Add color
            if (e.Row.RowIndex == 12)
                foreach (TableCell tc in e.Row.Cells)
                    if (tc != e.Row.Cells[0])
                        tc.Text = "<div class=\"upgrade-table-label\" style=\"background-color:" + tc.Text + ";\">&nbsp;</div>";


            //Hide AutoPay price if no autopay
            if (e.Row.RowIndex == 15)
                foreach (TableCell tc in e.Row.Cells)
                    if (tc != e.Row.Cells[0] && tc.Text.Contains("$0.000"))
                        tc.Text = "&nbsp;";

            if (OnlyOneMembership)
                if (e.Row.RowIndex == 1)
                    foreach (TableCell tc in e.Row.Cells)
                        tc.Text = "&nbsp;";

            //Disable Level Up properties for Standard Membership
            var levelUpProperties = new int[] { 44, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57 };
            if (levelUpProperties.Contains(e.Row.RowIndex))
                e.Row.Cells[1].Text = "N/A";

        }
        else if (e.Row.RowType == DataControlRowType.Header)
        {
            foreach (TableCell tc in e.Row.Cells)
                tc.Text = "&nbsp;";
        }
    }

    private void UpgradeMembership()
    {
        SuccMessagePanel.Visible = false;
        ErrorMessagePanel.Visible = false;

        try
        {
            AppSettings.DemoCheck();

            var pack = new MembershipPack(int.Parse(ddlOptions.SelectedValue));
            var user = Member.Current;
            var price = pack.GetPrice(user);
            var targetBalance = TargetBalanceRadioButtonList.TargetBalance;

            PurchaseOption.ValidateOperation(user, price, TargetBalanceRadioButtonList.Feature, targetBalance, TargetBalanceRadioButtonList.WalletCode);

            if (targetBalance == PurchaseBalances.PaymentProcessor)
            {
                string[] args = { user.Id.ToString(), price.ToClearString(), pack.Id.ToString() };
                TargetBalanceRadioButtonList.GeneratePaymentButtons(args);
                return;
            }

            Membership.BuyPack(user, pack, targetBalance, TargetBalanceRadioButtonList.WalletCode);

            Response.Redirect("~/status.aspx?type=upgradeok");
        }
        catch (MsgException ex)
        {
            ErrorMessagePanel.Visible = true;

            if (!TitanFeatures.IsRofriqueWorkMines)
                ErrorMessage.Text = ex.Message;
            else
                ErrorMessage.Text = "YOU DONT HAVE ENOUGH FUNDS IN YOUR CASH BALANCE. PLEASE CLICK ON DEPOSIT FUNDS TO TOP UP YOUR CASH BALANCE.";
        }
        catch (Exception ex)
        {
            ErrorLogger.Log(ex);
            throw ex;
        }
    }

    private void PurchaseBasePack()
    {
        SuccMessagePanel.Visible = false;
        ErrorMessagePanel.Visible = false;

        try
        {
            AppSettings.DemoCheck();

            var user = Member.Current;
            var project = new CrowdfundingProject(int.Parse(ProjectsDDL.SelectedValue));
            var pack = new BaseLifetimePack(int.Parse(BasePacksDDL.SelectedValue));
            var price = pack.GetPrice(user);
            var targetBalance = TargetBalanceBasePack.TargetBalance;

            PurchaseOption.ValidateOperation(user, price, TargetBalanceBasePack.Feature, targetBalance, TargetBalanceBasePack.WalletCode);

            if (targetBalance == PurchaseBalances.PaymentProcessor)
            {
                string[] args = { user.Id.ToString(), pack.Id.ToString(), project.Id.ToString() };
                TargetBalanceBasePack.GeneratePaymentButtons(args);
                return;
            }

            var note = user.BaseLifetimePackId != null ? "Quick Start Pack Upgrade" : "Quick Start Pack Purchase";
            PurchaseOption.ChargeBalance(user, price, TargetBalanceBasePack.Feature, targetBalance, note, TargetBalanceBasePack.WalletCode);

            BaseLifetimePack.PurchasePack(user, pack, project);

            Response.Redirect("~/status.aspx?type=upgradeok");
        }
        catch (MsgException ex)
        {
            ErrorMessagePanel.Visible = true;
            ErrorMessage.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ErrorLogger.Log(ex);
            throw ex;
        }
    }

    #region Commissions
    protected void BindCommissionsGridViewDataSource()
    {
        CommissionsGridView_DataSource.SelectCommand = string.Format("SELECT * FROM Commissions WHERE RefLevel <= {0} AND MembershipId = {1} ORDER BY RefLevel",
            AppSettings.Referrals.ReferralEarningsUpToTier, MembershipDDL.SelectedValue);
    }

    protected void CommissionsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        var row = e.Row;
        if (row.RowType == DataControlRowType.DataRow)
        {
            if (TitanFeatures.IsBitsor)
            {
                row.Cells[1].CssClass = "displaynone";
                CommissionsGridView.Columns[1].HeaderStyle.CssClass = "displaynone";
                row.Cells[16].CssClass = "displaynone";
                CommissionsGridView.Columns[16].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.AdvertBannersEnabled || (!AppSettings.TitanModules.HasProduct(2) && !AppSettings.TitanModules.HasProduct(3)))
            {
                row.Cells[2].CssClass = "displaynone";
                CommissionsGridView.Columns[2].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.AdvertAdPacksEnabled)
            {
                row.Cells[3].CssClass = "displaynone";
                CommissionsGridView.Columns[3].HeaderStyle.CssClass = "displaynone";
                row.Cells[4].CssClass = "displaynone";
                CommissionsGridView.Columns[4].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.Points.PointsEnabled || AppSettings.IsDemo)
            {
                row.Cells[4].CssClass = "displaynone";
                CommissionsGridView.Columns[4].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.EarnOfferwallsEnabled)
            {
                row.Cells[5].CssClass = "displaynone";
                CommissionsGridView.Columns[5].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.EarnCPAGPTEnabled || TitanFeatures.IsLibraryadvertising)
            {
                row.Cells[6].CssClass = "displaynone";
                CommissionsGridView.Columns[6].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.AdvertTrafficGridEnabled || AppSettings.IsDemo)
            {
                row.Cells[7].CssClass = "displaynone";
                CommissionsGridView.Columns[7].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.Payments.CashBalanceEnabled || HideCashBalanceDepositCommissionColumn)
            {
                row.Cells[8].CssClass = "displaynone";
                CommissionsGridView.Columns[8].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanModules.HasProduct(2) || !AppSettings.TitanFeatures.EarnVideoEnabled || TitanFeatures.IsLibraryadvertising)
            {
                row.Cells[9].CssClass = "displaynone";
                CommissionsGridView.Columns[9].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.InvestmentPlatformEnabled)
            {
                row.Cells[10].CssClass = "displaynone";
                CommissionsGridView.Columns[10].HeaderStyle.CssClass = "displaynone";
                row.Cells[11].CssClass = "displaynone";
                CommissionsGridView.Columns[11].HeaderStyle.CssClass = "displaynone";
            }
            if (TitanFeatures.IsCryptoRevolution)
            {
                row.Cells[11].CssClass = "displaynone";
                CommissionsGridView.Columns[11].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.Registration.IsAccountActivationFeeEnabled)
            {
                row.Cells[12].CssClass = "displaynone";
                CommissionsGridView.Columns[12].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.JackpotPvpEnabled)
            {
                row.Cells[13].CssClass = "displaynone";
                CommissionsGridView.Columns[13].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanModules.HasProduct(TitanProduct.ICO) || TitanFeatures.IsSimao)
            {
                row.Cells[14].CssClass = "displaynone";
                CommissionsGridView.Columns[14].HeaderStyle.CssClass = "displaynone";
                row.Cells[15].CssClass = "displaynone";
                CommissionsGridView.Columns[15].HeaderStyle.CssClass = "displaynone";
            }
            if (AppSettings.Payments.CurrencyMode == CurrencyMode.YourCoin)
            {
                row.Cells[14].CssClass = "displaynone";
                CommissionsGridView.Columns[14].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.MoneyDepositEnabled || !AppSettings.Points.PointsEnabled || AppSettings.IsDemo)
            {
                row.Cells[16].CssClass = "displaynone";
                CommissionsGridView.Columns[16].HeaderStyle.CssClass = "displaynone";
            }
            if (!TitanFeatures.IsCligence)
            {
                row.Cells[17].CssClass = "displaynone";
                CommissionsGridView.Columns[17].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.EarnYouTubeEnabled)
            {
                row.Cells[18].CssClass = "displaynone";
                CommissionsGridView.Columns[18].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.EarnFollowsEnabled)
            {
                row.Cells[19].CssClass = "displaynone";
                CommissionsGridView.Columns[19].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.EarnLikesEnabled)
            {
                row.Cells[20].CssClass = "displaynone";
                CommissionsGridView.Columns[20].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanModules.HasProduct(TitanProduct.SharesMarket) ||
                !AppSettings.TitanFeatures.SharesMarketExchangeEnabled ||
                AppSettings.SharesExchange.Mode == SharesMarketMode.Trading)
            {
                row.Cells[21].CssClass = "displaynone";
                CommissionsGridView.Columns[21].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.EarnYouTubeSubscriptionsEnabled)
            {
                row.Cells[22].CssClass = "displaynone";
                CommissionsGridView.Columns[22].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.EarnGoogleReviewsEnabled)
            {
                row.Cells[23].CssClass = "displaynone";
                CommissionsGridView.Columns[23].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.AdvertPaidEmailsEnabled)
            {
                row.Cells[24].CssClass = "displaynone";
                CommissionsGridView.Columns[24].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.MarketplaceEnabled)
            {
                row.Cells[25].CssClass = "displaynone";
                CommissionsGridView.Columns[25].HeaderStyle.CssClass = "displaynone";
            }
            if (!AppSettings.TitanFeatures.CrowdfundingProjectsEnabled)
            {
                row.Cells[26].CssClass = "displaynone";
                CommissionsGridView.Columns[26].HeaderStyle.CssClass = "displaynone";
            }

            for (int i = 1; i <= row.Cells.Count - 1; i++)
                row.Cells[i].Text = NumberUtils.FormatPercents(row.Cells[i].Text);
        }
        CommissionsGridView.Columns[0].HeaderText = U5009.TIER;
        CommissionsGridView.Columns[1].HeaderText = U5009.UPGRADEPURCHASE;
        CommissionsGridView.Columns[2].HeaderText = U5009.BANNERPURCHASE;
        CommissionsGridView.Columns[3].HeaderText = string.Format("{0} ({1})", string.Format(U5009.ADPACKPURCHASE, AppSettings.RevShare.AdPack.AdPackName), AppSettings.Site.CurrencyCode);
        CommissionsGridView.Columns[4].HeaderText = string.Format("{0} ({1})", string.Format(U5009.ADPACKPURCHASE, AppSettings.RevShare.AdPack.AdPackName), AppSettings.PointsName);
        CommissionsGridView.Columns[5].HeaderText = U5009.OFFERWALLS;
        CommissionsGridView.Columns[6].HeaderText = "CPA/GPT";
        CommissionsGridView.Columns[7].HeaderText = U5009.TRAFFICGRIDPURCHASE;
        CommissionsGridView.Columns[8].HeaderText = U6005.CASHBALANCEDEPOSIT;
        CommissionsGridView.Columns[9].HeaderText = U6014.VIDEOVIEW;
        CommissionsGridView.Columns[10].HeaderText = U6014.INVESTMENTPLANPURCHASE;
        CommissionsGridView.Columns[11].HeaderText = U6014.INVESTMENTPLANDAILYROI;
        CommissionsGridView.Columns[12].HeaderText = U6014.ACCOUNTACTIVATIONFEE;
        CommissionsGridView.Columns[13].HeaderText = U6014.JACKPOTPVPSTAGEPURCHASE;
        CommissionsGridView.Columns[14].HeaderText = string.Format("{0} ({1})", U6014.ICOPURCHASE, AppSettings.Cryptocurrencies.MyCoinCode);
        CommissionsGridView.Columns[15].HeaderText = string.Format("{0} ({1})", U6014.ICOPURCHASE, AppSettings.Payments.CurrencyMode == CurrencyMode.YourCoin ? AppSettings.Cryptocurrencies.MyCoinCode : AppSettings.Site.CurrencyCode);
        CommissionsGridView.Columns[16].HeaderText = string.Format("{0} ({1})", U4200.DEPOSIT, AppSettings.PointsName);
        CommissionsGridView.Columns[17].HeaderText = string.Format("{0} ({1})", U6019.EXTERNALSEARCH, AppSettings.Cryptocurrencies.MyCoinCode);
        CommissionsGridView.Columns[18].HeaderText = U6021.YOUTUBEVIEW;
        CommissionsGridView.Columns[19].HeaderText = U6023.TWITTERFOLLOW;
        CommissionsGridView.Columns[20].HeaderText = U6023.FACEBOOKLIKE;
        CommissionsGridView.Columns[21].HeaderText = U6023.SHARESMARKETBET;
        CommissionsGridView.Columns[22].HeaderText = U6023.YOUTUBESUBSCRIPTIONS;
        CommissionsGridView.Columns[23].HeaderText = U6025.GOOGLEREVIEWS;
        CommissionsGridView.Columns[24].HeaderText = U6026.PAIDTOREADEMAILS;
        CommissionsGridView.Columns[25].HeaderText = U6026.MARKETPLACEPURCHASE;
        CommissionsGridView.Columns[26].HeaderText = U6026.CROWDFUNDING;
    }

    private bool AreAllCashBalanceDepositCommissionsZero()
    {
        List<Commission> commissions = (List<Commission>)new CommissionsCache().Get();
        decimal sumOfCashBalanceCommissions = 0;
        foreach (var commission in commissions)
        {
            sumOfCashBalanceCommissions += commission.CashBalanceDepositPercent;
        }
        return Decimal.Equals(sumOfCashBalanceCommissions, 0);
    }

    protected void MembershipDDL_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCommissionsGridViewDataSource();
    }

    protected void BindDataToMembershipsDDL()
    {
        var list = new Dictionary<string, string>();
        var memberships = TableHelper.SelectRows<Membership>(TableHelper.MakeDictionary("Status", (int)MembershipStatus.Active));

        for (int i = 0; i < memberships.Count; i++)
        {
            if ((TitanFeatures.IsSimao && memberships[i].Id == Membership.Standard.Id) ||
                (memberships[i].IsGeolocated && !memberships[i].GeolocatedCC.Contains(Member.CurrentInCache.CountryCode)))
                continue;

            list.Add(memberships[i].Id.ToString(), memberships[i].Name);
        }

        MembershipDDL.DataSource = list;
        MembershipDDL.DataTextField = "Value";
        MembershipDDL.DataValueField = "Key";
        MembershipDDL.DataBind();
    }

    protected void MembershipDDL_Init(object sender, EventArgs e)
    {
        BindDataToMembershipsDDL();
    }

    protected void CommissionsGridView_DataSource_Load(object sender, EventArgs e)
    {
        BindCommissionsGridViewDataSource();
    }
    #endregion
}