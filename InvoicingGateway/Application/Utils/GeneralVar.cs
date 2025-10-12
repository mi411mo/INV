using System;
using Domain.Models;
using Tharwat.Switch.Utilites.Application.Common.SystemEnviornment;

namespace Application.Utils
{
    public static class GeneralVar
    {
        public static string GetURL()
    {
        return ReadConfigurationFile("ExternalUrl", "ProviderURL");
    }
    public static string GetUserPermissionByUserURL()
    {
        var permissionUrlEnvName = ReadConfigurationFile("EnviornmentLookup", "GetUserPermissionByUserURL");
        var basePermissionUrl = SystemEnviornmentLookup.GetEnvVariableValue(permissionUrlEnvName);
        var permissionUrl = basePermissionUrl.Trim() + "/api/Users/GetUserPermissionByUserId?userId=";
        return permissionUrl; 
        //return ReadConfigurationFile("EnviornmentLookup", "GetUserPermissionByUserURL");
    }
    public static string GetLookupURL()
    {
        return ReadConfigurationFile("LookupInformation", "LookupURL");
    }
    public static string GetProviderCode()
    {
        return ReadConfigurationFile("LookupInformation", "ProviderCode");
    }
    public static string GetProviderId()
    {
        return ReadConfigurationFile("MySettings", "DefultProviderId");
    }

    public static bool GetProviderfromRequest()
    {
        var runMode = ReadConfigurationFile("MySettings", "GetProviderfromRequest");
        if (runMode != null && runMode.Equals("False"))
            return false;
        return true;
    }
    public static string GetProviderName()
    {
        return ReadConfigurationFile("MySettings", "DefultProviderName");
    }
    public static string GetLinkProviderId()
    {
        return ReadConfigurationFile("MySettings", "LinkProviderId");
    }
    public static bool CheckCategory()
    {
        var runMode = ReadConfigurationFile("MySettings", "CheckCategory");
        if (runMode != null && runMode.Equals("False"))
            return false;
        return true;
    }
    public static bool IsShouldProviderSatusActive()
    {
        var result = ReadConfigurationFile("MySettings", "IsShouldProviderSatusActive");
        if (result != null)
            return Convert.ToBoolean(result);
        return false;
    }

    public static bool PerformGetProviderId()
    {
        var runMode = ReadConfigurationFile("MySettings", "PerformGetProviderId");
        if (runMode != null && runMode.Equals("False"))
            return false;
        return true;
    }
    public static string GetAdminCustomerId()
    {
        try
        {
            if (AllowAdminProcess())
                return ReadConfigurationFile("MySettings", "AdminCustomerId");
            return string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }
    public static bool AllowAdminProcess()
    {
        var runMode = ReadConfigurationFile("MySettings", "AllowAdminProcess");
        if (runMode != null && runMode.Equals("False"))
            return false;
        return true;
    }
    public static bool GetAllData()
    {
        var runMode = ReadConfigurationFile("MySettings", "GetAllData");
        if (runMode != null && runMode.Equals("false"))
            return false;
        return true;
    }
    public static string GetBaseGatewayURL()
    {
        var variableName = ReadConfigurationFile("EnviornmentLookup", "WalletsBankstMgmtPayGWURLVariable");
        var gatewayURL = SystemEnviornmentLookup.GetEnvVariableValue(variableName);
        if (!string.IsNullOrWhiteSpace(gatewayURL))
            return gatewayURL;
        else
            return ReadConfigurationFile("Gateway", "BaseURL");
    }
    public static string GetWalletsBankstMgmtSTSURL()
    {
        var STSVariableName = ReadConfigurationFile("EnviornmentLookup", "WalletsBankstMgmtSTSURLVariable");
        return Tharwat.Switch.Utilites.Application.Common.SystemEnviornment.SystemEnviornmentLookup.GetEnvVariableValue(STSVariableName) + ReadConfigurationFile("Gateway", "STSURL");
    }
    public static string GetApplicationId()
    {
        return ReadConfigurationFile("Gateway", "ApplicationId");
    }
    public static string GetApplicationSecret()
    {
        return ReadConfigurationFile("Gateway", "ApplicationSecret");
    }
    public static string GetGrantType()
    {
        return ReadConfigurationFile("Gateway", "GrantType");
    }
    public static string GetScope()
    {
        return ReadConfigurationFile("Gateway", "Scope");
    }
    public static string ReadConfigurationFile(string section, string ChildSection)
    {
        try
        {
            var value = ConfigHelper.Configuration.GetSection(section)[ChildSection];
                if (string.IsNullOrWhiteSpace(value) || value.Length <= 0)
                    throw null ;
            return value;
        }
        catch (Exception ex)
        {
                throw;
        }
    }

    public static bool Run_TestMode()
    {
        var runMode = ConfigHelper.Configuration.GetSection("MySettings")["TestMode"];
        if (runMode != null && runMode.Equals("0"))
            return false;
        return true;
    }
    public static bool Run_TranasctionStatuesSuccess()
    {
        var runMode = ConfigHelper.Configuration.GetSection("TranasctionStatues")["Success"];
        if (runMode != null && runMode.Equals("0"))
            return false;
        return true;
    }
    public static bool SuccessResultTestMode(out string errorCode)
    {
        errorCode = null;
        var runMode = ConfigHelper.Configuration.GetSection("MySettings")["SuccessResultTestMode"];
        if (runMode != null && runMode.Equals("0"))
        {
            errorCode = GetErrorCode() ?? "7003";// ReadConfigurationFile("MySettings", "ErrorCode");
            return false;
        }
        return true;
    }
    public static string GetErrorCode()
    {
        return ConfigHelper.Configuration.GetSection("MySettings")["ErrorCode"];
    }
    public static string GetCodeURL()
    {
        return ReadConfigurationFile("AutherizationCode", "APIUrl");
    }
    public static string GetNotificationURL()
    {
        return ReadConfigurationFile("Notification", "APIUrl");
    }

    public static string GetTSPSide()
    {
        return ReadConfigurationFile("TSPSide", "Provider");
    }
    public static string GetPaymentURL()
    {
        return ReadConfigurationFile("MySettings", "PaymentURL");
    }
    public static string GetPaymentsMethodesURL()
    {
        return ReadConfigurationFile("MySettings", "PaymentsMethodes");
    }
    public static string GetLinkCheckoutURL()
    {
        return ReadConfigurationFile("MySettings", "LinkCheckout");
    }
    public static string GetProfilesURL()
    {
        return ReadConfigurationFile("MySettings", "ProfilesURL");
    }
    //public static string GetUpdateProfilesURL()
    //{
    //    return ReadConfigurationFile("MySettings", "UpdateProfilesURL");
    //}  
    public static string GetPostURL()
    {
        return ReadConfigurationFile("MySettings", "PostURL");
    }
    public static bool GetUrlFromPost()
    {
        var data = ReadConfigurationFile("MySettings", "CheckUrl");
        if (data == "true")
            return true;
        return false;
    }
    public static string GetRedirectURL()
    {
        return ReadConfigurationFile("MySettings", "RedirectURL");
    }
    public static bool IsTSPSideWallet()
    {
        bool res = false;
        var data = ReadConfigurationFile("TSPSide", "Provider");
        if (data == "Wallet")
            res = true;
        return res;
    }
    public static int GetTakePagination()
    {
        var validityPeriod_ = ReadConfigurationFile("Pagination", "Take");
        if (!string.IsNullOrWhiteSpace(validityPeriod_))
            return int.Parse(validityPeriod_);
        return 10;

    }

    public static string GetTSPSide2()
    {
        return ReadConfigurationFile("TSPSide2", "Provider");
    }


    }
}
