using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.CustomerWebService.Core
{
    public static class Constants
    {
        public const long CrcIdMax = 980000000000000;
        public const string BannerStaticKeyWD = "42099111111"; //
        public const string BannerStaticKeyBilo = "44022222222";
        public const string BannerStaticKeyHarveys = "44193333333";

        public const int CrcIdMaxLength = 18;
        public const int AddressLine1MaxLength = 200;
        public const int AddressLine2MaxLength = 200;
        public const int CityMaxLength = 40;
        public const int StateMaxLength = 6;
        public const int CountryMaxLength = 2;
        public const int PostalCodeMaxLength = 12;

        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;
        public const int SalutationMaxLength = 10;
        public const int EmailMaxLength = 255;
        public const int PhoneMaxLength = 49;
        public const int ChainIdMaxLength = 2;
        public const int LanguageCodeMaxLength = 2;
        public const int LastUpdateSourceMaxLength = 50;

        public const int AccountIdMaxLength = 36;
        public const int AliasNumberMaxLength = 60;
        public const int AliasCardMaxLength = 36;
        public const int EnrollmentCardMaxLength = 36;
        public const int AttributeCodeMaxLength = 100;
        public const int MemberIdMaxLength = 36;
        public const int ReferenceIdMaxLength = 36;

        public const int AgeCutOff = 2;
        public const int DefaultMaxNumberOfSearchResults = 25;

        public const long BeginningSequenceForRainChecks = 42999990000;
        public const long EndingSequenceForRainChecks = 42999999999;

        public const string CustPreferenceMemberId = "MEMBER_ID";
        public const string CustPreferenceBanner = "BANNER";
        public const string ResponseErrorMessage = "An error occurred while processing the search request, please try again.";
        public const string ErrorXmlTag = "<Error>";
        public const string ErrorStatus = "Error";
        public const string UpsertDelete = "OmniCustomer/UpsertDelete";
        public const string Upsert = "CUSTODS/UPSERT";
        public const string UpsertAttribute = "OMNI/AttributeUpsert";
        public const string UpsertAlias = "OMNI/AliasUpsert";
        public const string UpsertCred = "/CUSTODS/CREDUPSERT";
        public const string CustExtendedUpsert = "OMNI/CustExtendedUpsert";
        public const string CustExtendedRetrieve = "OMNI/CustExtendedRetrieve";
        public const string UpdateMobileResource = "OMNICHANNEL/SetValidatedPhoneEmail";//"OMNI/SetValidatedPhoneEmail";
        public const string RetrievePreference = "CUSTODS/PREFERENCES/RETRIEVE";
        public const string RetrieveUpsert = "OMNI/PreferenceUpsert";
        public const string RetrievePrimaryStore = "OMNICHANNEL/PRIMARYSTORE/RETRIEVE";

        public const string CustSearchHard = "/CUSTODS/SEARCH/HARDSEARCH";
        public const string CustAliasRetrieve = "/CUSTODS/CUSTALIAS/RETRIEVE";
        public const string CustSearch = "/CUSTODS/CUST/SEARCH";
        public const string CustSearchFuzzy = "/CUSTODS/SEARCH/FUZZYSEARCH";
        public const string CustPhoneValidation = "CUSTODS/INVALID_PHONE/VALIDATION";
        public const string OmniFRNStatusValidate = "OMNICHANNEL/FRNStatusValidate";

        public const string BadParameterLengthErrorMessage = "{0} has exceeded the max length allowed.Max Length: {1}, Current Lenght: {2}, Current Value: {3}";
        public const string InvalidPasswordValue = "{0} Is not a valid Encrypted PIN value.";

        public const string CrcIdDeleteParameter = "crcid";
        public const string ChildIDDeleteParameter = "ChildID";
        public const string MemberIDDeleteParameter = "MemberID";
        public const string AttributeIDDeleteParameter = "CustomerAttr";

        public const string MEMBER_IDSearchParameter = "MEMBER_ID";
        public const string CRC_IDSearchParameter = "CRC_ID";
        public const string ALIAS_NUMBERSearchParameter = "ALIAS_NUMBER";
        public const string EMAIL_IDSearchParameter = "EMAIL_ID";

        public const string CHAIN_IDSearchParameter = "CHAIN_ID";
        public const string ROWSSearchParameter = "ROWS";

        public const string FIRST_NAMESearchParameter = "FIRST_NAME";
        public const string LAST_NAMESearchParameter = "LAST_NAME";
        public const string STREET_ADDRESS_1SearchParameter = "STREET_ADDRESS_1";
        public const string STREET_ADDRESS_2SearchParameter = "STREET_ADDRESS_2";
        public const string CITY_NAMESearchParameter = "CITY_NAME";
        public const string STATE_CODESearchParameter = "STATE_CODE";
        public const string POSTAL_CODESearchParameter = "POSTAL_CODE";
        public const string ACCOUNT_IDSearchParameter = "ACCOUNT_ID";
        public const string MOBILE_PHONESearchParameter = "MOBILE_PHONE";

        public const string MemberIdMemberCheckParameter = "MemberID";

        public const string StaticCrcIdAppSettingKey = "StaticCrcId";

        public const string CustomerFirstName = "FirstName";
        public const string CustomerLastName = "LastName";
        public const string CustomerSaluation = "Salutation";
        public const string CustomerEmailAddress = "EmailAddress";
        public const string CustomerMobilePhoneNumber = "MobilePhoneNumber";
        public const string CustomerLanguageCode = "LanguageCode";
        public const string CustomerMemberId = "MemberId";
        public const string CustomerAddressLine1 = "AddressLine1";
        public const string CustomerAddressLine2 = "AddressLine2";
        public const string CustomerCity = "City";
        public const string CustomerState = "State";
        public const string CustomerCountry = "Country";
        public const string CustomerPostalCode = "PostalCode";

        public const string WinnDixieCouponAliasPrefix = "9800";
        public const string WinnDixieCRCIdPrefix = "9800";
        public const string FRNCardPrefix = "722";
        public const string GGCardNumberPrefix = "7221";
    }
}
