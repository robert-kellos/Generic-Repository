

namespace SampleArch.Utilities
{
    /// <summary>
    ///     AppConstant
    /// </summary>
    public class AppConstant
    {
        /// <summary>
        ///     DEFAULT
        /// </summary>
        public struct DEFAULT
        {
            /// <summary>
            ///     The server
            /// </summary>
            public const string WMI_SERVER = "cpwiitgq";

            /// <summary>
            ///     The macaddress
            /// </summary>
            public const string MACADDRESS = "00:1A:D4:10:A8:C8";
        }

        /// <summary>
        /// </summary>
        public struct FILE_INFO
        {
            /// <summary>
            ///     The dvs.json file
            /// </summary>
            public const string DVS_JSON = @"\dvs.json";

            /// <summary>
            ///     The hardware.json file
            /// </summary>
            public const string HARDWARE_JSON = @"\hardware.json";

            /// <summary>
            ///     The dvs_added.json file
            /// </summary>
            public const string DVS_ADDED_JSON = @"\dvs_added.json";
        }

        /// <summary>
        ///     FOLDER_INFO
        /// </summary>
        public struct FOLDER_INFO
        {
            /// <summary>
            ///     The custom folder for sorting hat
            /// </summary>
            public const string CUSTOM = @"\Custom";

            /// <summary>
            ///     The updated folder for sorting hat
            /// </summary>
            public const string UPDATED = @"\Updated";

            /// <summary>
            ///     The backup folder for sorting hat
            /// </summary>
            public const string BAK = @"\bak";
        }

        /// <summary>
        ///     BUILD_DATA_KEY
        /// </summary>
        public struct BUILD_DATA_KEY
        {
            /// <summary>
            ///     The DG = Default Gateway key
            /// </summary>
            public const string DG = "DG";
        }

        /// <summary>
        ///     SITE_VARIABLE
        /// </summary>
        public struct SITE_VARIABLE
        {
            /// <summary>
            ///     THDSITENUMBER
            /// </summary>
            public const string THIS_SITE_NUMBER = "THDSITENUMBER";

            /// <summary>
            ///     LOCATION
            /// </summary>
            public const string LOCATION = "LOCATION";

            /// <summary>
            ///     The KEYBOARDLOCALE
            /// </summary>
            public const string KEYBOARDLOCALE = "KEYBOARDLOCALE";

            /// <summary>
            ///     The INPUTLOCALE
            /// </summary>
            public const string INPUTLOCALE = "INPUTLOCALE";

            /// <summary>
            ///     The SYSTEMLOCALE
            /// </summary>
            public const string SYSTEMLOCALE = "SYSTEMLOCALE";

            /// <summary>
            ///     The TIMEZONE
            /// </summary>
            public const string TIMEZONE = "TIMEZONE";

            /// <summary>
            ///     The TIMEZONENAME
            /// </summary>
            public const string TIMEZONENAME = "TIMEZONENAME";

            /// <summary>
            ///     The UILANGUAGE
            /// </summary>
            public const string UILANGUAGE = "UILANGUAGE";

            /// <summary>
            ///     The USERLOCALE
            /// </summary>
            public const string USERLOCALE = "USERLOCALE";

            /// <summary>
            ///     The UDSHARE
            /// </summary>
            public const string UDSHARE = "UDSHARE";
        }

        /// <summary>
        ///     HEADER_KEY
        /// </summary>
        public struct HEADER_KEY
        {
            /// <summary>
            ///     The content-type header key
            /// </summary>
            public const string CONTENT_TYPE = "content-type";
        }

        /// <summary>
        ///     HEADER_VALUE
        /// </summary>
        public struct HEADER_VALUE
        {
            /// <summary>
            ///     The application/json header value
            /// </summary>
            public const string APPLICATION_JSON = "application/json";
        }

        /// <summary>
        ///     APPSETTING
        /// </summary>
        public struct APPSETTING
        {
            /// <summary>
            ///     The Url appsetting key
            /// </summary>
            public const string Url = "url";

            /// <summary>
            ///     The common file store key
            /// </summary>
            public const string COMMON_FILE_STORE_KEY = "CommonFileStore";
        }

        /// <summary>
        ///     WEBSERVICE_ACTION
        /// </summary>
        public struct WEBSERVICE_ACTION
        {
            /// <summary>
            ///     The get
            /// </summary>
            public const string GET = "GET";

            /// <summary>
            ///     The post
            /// </summary>
            public const string POST = "POST";

            /// <summary>
            ///     The put
            /// </summary>
            public const string PUT = "PUT";

            /// <summary>
            ///     The delete
            /// </summary>
            public const string DELETE = "DELETE";
        }

        /// <summary>
        ///     PARAMNAME
        /// </summary>
        public struct PARAMNAME
        {
            /// <summary>
            ///     The computer name parameter
            /// </summary>
            public const string COMPUTERNAME = @"ComputerName";

            /// <summary>
            ///     The namespace parameter
            /// </summary>
            public const string NAMESPACE = @"Namespace";

            /// <summary>
            ///     The class parameter
            /// </summary>
            public const string CLASS = @"Class";

            /// <summary>
            ///     The filter parameter
            /// </summary>
            public const string FILTER = @"Filter";

            /// <summary>
            ///     The Path parameter
            /// </summary>
            public const string PATH = @"Path";
        }

        /// <summary>
        ///     COMMAND
        /// </summary>
        public struct COMMAND
        {
            /// <summary>
            ///     The gwmi wmi command
            /// </summary>
            public const string GWMI = @"gwmi";

            /// <summary>
            ///     The SMS_R_SYSTEM class command
            /// </summary>
            public const string SMS_R_SYSTEM = @"SMS_R_SYSTEM";

            /// <summary>
            ///     The root\sms\site_THD namespace command
            /// </summary>
            public const string ROOT_SMS_SITE_THD = @"root\sms\site_THD";

            /// <summary>
            ///     The mac addresses command
            /// </summary>
            public const string MACADDRESSES = @"MACAddresses";

            /// <summary>
            ///     The ConvertTo-Json command
            /// </summary>
            public const string CONVERT_TO_JSON = @"ConvertTo-Json";

            /// <summary>
            ///     The Import-Clixml command
            /// </summary>
            public const string IMPORT_CLIXML = "Import-Clixml";

            /// <summary>
            ///     The WIN32_NETWORKADAPTERCONFIGURATION
            /// </summary>
            public const string WIN32_NETWORKADAPTERCONFIGURATION = "win32_networkadapterconfiguration";
        }

        /// <summary>
        ///     The space
        /// </summary>
        public const string Space = " ";

        /// <summary>
        ///     Provides static access to the name of custom headers used in the AllTheSame Application
        /// </summary>
        public static class CustomHeaderCode
        {
            /// <summary>
            ///     Designates the Organization Context used for the Web Method call
            ///     All Permissions are granted in the context of the specified Organization
            /// </summary>
            public const string OrgId = "orgId";
        }

        /// <summary>
        ///     PermissionCode
        /// </summary>
        public static class PermissionCode
        {
            /// <summary>
            ///     The register vendor
            /// </summary>
            public const string RegisterVendor = "reg_vendor";

            /// <summary>
            ///     The add vendor
            /// </summary>
            public const string AddVendor = "add_vendor";

            /// <summary>
            ///     The view vendor
            /// </summary>
            public const string ViewVendor = "view_vendor";

            /// <summary>
            ///     The edit vendor
            /// </summary>
            public const string EditVendor = "edit_vendor";

            /// <summary>
            ///     The archive vendor
            /// </summary>
            public const string ArchiveVendor = "archive_vendor";

            /// <summary>
            ///     The purge vendor
            /// </summary>
            public const string PurgeVendor = "purge_vendor";

            /// <summary>
            ///     The register community
            /// </summary>
            public const string RegisterCommunity = "reg_community";

            /// <summary>
            ///     The add community
            /// </summary>
            public const string AddCommunity = "add_community";

            /// <summary>
            ///     The view community
            /// </summary>
            public const string ViewCommunity = "view_community";

            /// <summary>
            ///     The edit community
            /// </summary>
            public const string EditCommunity = "edit_community";

            /// <summary>
            ///     The archive community
            /// </summary>
            public const string ArchiveCommunity = "archive_community";

            /// <summary>
            ///     The purge community
            /// </summary>
            public const string PurgeCommunity = "purge_community";
        }

        /// <summary>
        ///     ErrorMessages
        /// </summary>
        public static class ErrorMessages
        {
            /// <summary>
            ///     The na
            /// </summary>
            public const string NA = "N/A";

            /// <summary>
            ///     The not available
            /// </summary>
            public const string NotAvailable = "Not available";

            /// <summary>
            ///     The no results
            /// </summary>
            public const string NoResults = "No results";

            /// <summary>
            ///     The unable to save
            /// </summary>
            public const string UnableToSave = "Unable to save";

            /// <summary>
            ///     The unable to edit
            /// </summary>
            public const string UnableToEdit = "Unable to edit";

            /// <summary>
            ///     The application error
            /// </summary>
            public const string ApplicationError = "Application Error";

            /// <summary>
            ///     The internal server error
            /// </summary>
            public const string InternalServerError = "Internal Server Error";

            /// <summary>
            ///     The cannot be null
            /// </summary>
            public const string CannotBeNull = "{0} cannot be null.";

            /// <summary>
            ///     should not be empty
            /// </summary>
            public const string ShouldNotBeEmpty = "{0} should not be empty.";

            /// <summary>
            ///     must be a valid email
            /// </summary>
            public const string MustBeValidEmail = "{0} must be a valid email.";

            /// <summary>
            ///     must be a valid phone number
            /// </summary>
            public const string MustBeValidPhoneNumber = "{0} must be a valid phone number.";

            /// <summary>
            ///     The database update concurrency exception message
            /// </summary>
            public const string DbUpdateConcurrencyExceptionMessage = "DbUpdateConcurrencyException";

            /// <summary>
            ///     The database entity validation exception message
            /// </summary>
            public const string DbEntityValidationExceptionMessage = "DbEntityValidationException";

            /// <summary>
            ///     The database update exception message
            /// </summary>
            public const string DbUpdateExceptionMessage = "DbUpdateException";

            /// <summary>
            ///     The database exception message
            /// </summary>
            public const string DbExceptionMessage = "Db_Exception";

            /// <summary>
            ///     The SQL exception message
            /// </summary>
            public const string SqlExceptionMessage = "SqlException";

            /// <summary>
            ///     The argument null exception message
            /// </summary>
            public const string ArgumentNullExceptionMessage = "{0} - ArgumentNullException";

            /// <summary>
            ///     The null reference exception message
            /// </summary>
            public const string NullReferenceExceptionMessage = "{0} - NullReferenceException";

            /// <summary>
            ///     The exception message
            /// </summary>
            public const string ExceptionMessage = "Exception raised";

            /// <summary>
            ///     The exception message
            /// </summary>
            public const string UnhandledExceptionMessage = "UnhandledException raised";

            /// <summary>
            ///     The dispose message
            /// </summary>
            public const string DisposeMessage = "{0} - Disposed";
        }

    }
}