////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	AutoMapperConfig.cs
//
// summary:	Implements the automatic mapper configuration class
////////////////////////////////////////////////////////////////////////////////////////////////////

using AutoMapper;
using SEG.ApiService.Models;
using SEG.CustomerWebService.Core.ExtensionMethods;
using SEG.LoyaltyService.Models.Results;
using System;
using System.Collections.Generic;

namespace SEG.CustomerWebService.Core
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An automatic mapper configuration. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class WebServiceMappingsProfile : Profile
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Registers the customer result to customer mappings. </summary>
        ///

        ///
        /// <returns>   A MapperConfiguration. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public WebServiceMappingsProfile()
        {
            var formatControl = new System.Globalization.NumberFormatInfo
            {
                NumberDecimalDigits = 0
            };

            CreateMap<CustomerAddressResult, CustomerAddress>()
                .IgnoreAllNullMembers()
                    .ForMember(dest => dest.MemberId,
                        opts => opts.MapFrom(src => src.MEMBER_ID.Trim()))
                    .ForMember(dest => dest.AddressType,
                        opts => opts.MapFrom(src => src.ADDRESS_TYPE))
                    .ForMember(dest => dest.AddressLine1,
                        opts => opts.MapFrom(src => src.STREET_ADDRESS_1.Trim()))
                    .ForMember(dest => dest.AddressLine2,
                        opts => opts.MapFrom(src => src.STREET_ADDRESS_2.Trim()))
                    .ForMember(dest => dest.City,
                        opts => opts.MapFrom(src => src.CITY_NAME.Trim()))
                    .ForMember(dest => dest.State,
                        opts => opts.MapFrom(src => src.STATE_CODE.Trim()))
                    .ForMember(dest => dest.Country,
                        opts => opts.MapFrom(src => src.COUNTRY_CODE.Trim()))
                    .ForMember(dest => dest.PostalCode,
                        opts => opts.MapFrom(src => src.POSTAL_CODE.Trim()))
                    .ForMember(dest => dest.LastUpdateSource,
                        opts => opts.MapFrom(src => src.LAST_UPDATE_SOURCE.Trim()))
                    .ForMember(dest => dest.LastUpdateDate,
                        opts =>
                        {
                            opts.Condition(src => src.LAST_UPDATE_DT != default(DateTime));
                            opts.MapFrom(src => src.LAST_UPDATE_DT);
                        });

               CreateMap<CustomerChildResult, CustomerChild>()
                .IgnoreAllNullMembers()
                    .ForMember(dest => dest.MemberId,
                        opts => opts.MapFrom(src => src.MEMBER_ID.ToString()))
                    .ForMember(dest => dest.ChildId,
                        opts => opts.MapFrom(src => src.CHLD_ID))
                    .ForMember(dest => dest.FirstName,
                        opts => opts.MapFrom(src => src.FIRST_NAME.Trim()))
                    .ForMember(dest => dest.LastName,
                        opts => opts.MapFrom(src => src.LAST_NAME.Trim()))
                    .ForMember(dest => dest.BirthDate,
                        opts =>
                        {
                            opts.Condition(src => src.BIRTH_DATE != default(DateTime));
                            opts.MapFrom(src => src.BIRTH_DATE);
                        })
                    .ForMember(dest => dest.Expected,
                        opts => opts.MapFrom(src => src.EXPECTED_BABY_IND.Trim()))
                    .ForMember(dest => dest.GenderCode,
                        opts => opts.MapFrom(src => src.GENDER_CODE.Trim()))
                    .ForMember(dest => dest.Deceased,
                        opts => opts.MapFrom(src => src.DECEASED_IND.Trim()))
                    .ForMember(dest => dest.SpecialNeeds,
                        opts => opts.MapFrom(src => src.SPECIAL_NEEDS_IND.Trim()))
                    .ForMember(dest => dest.AgedOut,
                        opts => opts.MapFrom(src => src.AGED_OUT_IND.Trim()))
                    .ForMember(dest => dest.LastUpdateSource,
                        opts => opts.MapFrom(src => src.LAST_UPDATE_SOURCE.Trim()))
                    .ForMember(dest => dest.LastUpdateDate,
                        opts =>
                        {
                            opts.Condition(src => src.LAST_UPDATE_DT != default(DateTime));
                            opts.MapFrom(src => src.LAST_UPDATE_DT);
                        });

               CreateMap<CustomerAliasResult, MemberAlias>()
                .IgnoreAllNullMembers()
                    .ForMember(dest => dest.AliasType,
                        opts => opts.MapFrom(src => src.ALIAS_TYPE))
                    .ForMember(dest => dest.AliasNumber,
                        opts => opts.MapFrom(src => src.ALIAS_NUMBER.Trim()))
                    
                    .ForMember(dest => dest.AliasStatus,
                        opts => opts.MapFrom(src => src.ALIAS_STATUS.Trim()))
                    .ForMember(dest => dest.MemberId,
                        opts => opts.MapFrom(src => src.MEMBER_ID.Trim()))
                    .ForMember(dest => dest.LastUpdateSource,
                        opts => opts.MapFrom(src => src.LAST_UPDATE_SOURCE.Trim()))
                    .ForMember(dest => dest.LastUpdateDate,
                        opts =>
                        {
                            opts.Condition(src => src.LAST_UPDATE_DT != default(DateTime));
                            opts.MapFrom(src => src.LAST_UPDATE_DT);
                        });

               CreateMap<CustomerCRCResult, CustomerCRC>()
                .IgnoreAllNullMembers()
                    .ForMember(dest => dest.MemberId,
                        opts => opts.MapFrom(src => src.MEMBER_ID.Trim()))
                    .ForMember(dest => dest.CrcId,
                        opts => opts.MapFrom(src => src.CRC_ID))
                    .ForMember(dest => dest.TaxExempt,
                        opts => opts.MapFrom(src => src.TAX_EXEMPT))
                    .ForMember(dest => dest.SeniorDiscount,
                        opts => opts.MapFrom(src => src.SENIOR_DISCOUNT))
                    
                    .ForMember(dest => dest.EnrollmentLocId,
                        opts => opts.MapFrom(src => src.ENROLLMENT_LOC_ID))
                    .ForMember(dest => dest.ChainId,
                        opts => opts.MapFrom(src => src.CHAIN_ID));

               CreateMap<CustomerWalletResult, CustomerWallet>()
                .IgnoreAllNullMembers()
                    .ForMember(dest => dest.MemberId,
                        opts => opts.MapFrom(src => src.MEMBER_ID.Trim()))
                    .ForMember(dest => dest.WalletId,
                        opts => opts.MapFrom(src => src.Wallet_ID))
                    .ForMember(dest => dest.ConsumerId,
                        opts => opts.MapFrom(src => src.Consumer_ID));


               CreateMap<CustomerResult, CustomerV2>()
                .IgnoreAllNullMembers()
                    .ForMember(dest => dest.MemberId,
                        opts => opts.MapFrom(src => src.MEMBER_ID.Trim()))

                    .ForMember(dest => dest.FirstName,
                        opts => opts.MapFrom(src => src.FIRST_NAME.Trim()))
                    .ForMember(dest => dest.LastName,
                        opts => opts.MapFrom(src => src.LAST_NAME.Trim()))
                    .ForMember(dest => dest.Salutation,
                        opts => opts.MapFrom(src => src.SALUTATION.Trim()))
                    .ForMember(dest => dest.BirthDate,
                        opts =>
                        {
                            opts.Condition(src => src.BIRTH_DT != default(DateTime));
                            opts.MapFrom(src => src.BIRTH_DT);
                        })
                    .ForMember(dest => dest.GenderCode,
                        opts => opts.MapFrom(src => src.GENDER_CODE.Trim()))
                    .ForMember(dest => dest.EmailAddress,
                        opts => opts.MapFrom(src => src.EMAIL_ADDRESS.Trim()))
                    .ForMember(dest => dest.MobilePhone,
                        opts => opts.MapFrom(src => src.MOBILE_PHONE.ToString(formatControl)))
                    .ForMember(dest => dest.EmailModificationDate,
                        opts =>
                        {
                            opts.Condition(src => src.EMAIL_MODIFICATION_DT != default(DateTime));
                            opts.MapFrom(src => src.EMAIL_MODIFICATION_DT);
                        })
                    .ForMember(dest => dest.MobileModificationDate,
                        opts =>
                        {
                            opts.Condition(src => src.MOBILE_MODIFICATION_DT != default(DateTime));
                            opts.MapFrom(src => src.MOBILE_MODIFICATION_DT);
                        })
                    .ForMember(dest => dest.CreatedDate,
                        opts =>
                        {
                            opts.Condition(src => src.CREATED_DT != default(DateTime));
                            opts.MapFrom(src => src.CREATED_DT);
                        })
                    .ForMember(dest => dest.LanguageCode,
                        opts => opts.MapFrom(src => src.LANGUAGE_CODE.Trim()))
                    .ForMember(dest => dest.LastUpdateSource,
                        opts => opts.MapFrom(src => src.LAST_UPDATE_SOURCE.Trim()))
                    .ForMember(dest => dest.LastUpdateDate,
                        opts =>
                        {
                            opts.Condition(src => src.LAST_UPDATE_DT != default(DateTime));
                            opts.MapFrom(src => src.LAST_UPDATE_DT);
                        })
                    .ForMember(dest => dest.EnrollmentStatus,
                        opts => opts.MapFrom(src => src.ENROLLMENT_STATUS.Trim()))
                    .ForMember(dest => dest.EnrollmentDate,
                        opts =>
                        {
                            opts.Condition(src => src.ENROLLMENT_DATE != default(DateTime));
                            opts.MapFrom(src => src.ENROLLMENT_DATE);
                        })
                    .ForMember(dest => dest.LastShoppedStore,
                        opts => opts.MapFrom(src => src.LAST_SHOPPED_STORE))
                   .ForMember(dest => dest.LastShopDate,
                        opts =>
                        {
                            opts.Condition(src => src.LAST_SHOP_DATE != default(DateTime));
                            opts.MapFrom(src => src.LAST_SHOP_DATE);
                        })
                    .ForMember(dest => dest.EnrollmentBanner,
                        opts => opts.MapFrom(src => src.ENROLLMENT_BANNER.Trim()))
                    .ForMember(dest => dest.SelfSelectedStoreWD,
                        opts => opts.MapFrom(src => src.SELF_SELECTED_STORE_WD))
                    .ForMember(dest => dest.SelfSelectedStoreBL,
                        opts => opts.MapFrom(src => src.SELF_SELECTED_STORE_BL))
                    .ForMember(dest => dest.SelfSelectedStoreHvy,
                        opts => opts.MapFrom(src => src.SELF_SELECTED_STORE_Hvy))
                    .ForMember(dest => dest.SelfSelectedStoreFYM,
                        opts => opts.MapFrom(src => src.SELF_SELECTED_STORE_FYM))
                    .ForMember(dest => dest.AccountPin,
                        opts => opts.MapFrom(src => src.ACCOUNT_PIN.Trim()))
                    .ForMember(dest => dest.InvalidPinAttempts,
                        opts => opts.MapFrom(src => src.INVALID_PIN_ATTEMPTS))
                    .ForMember(dest => dest.PinLockout,
                        opts => opts.MapFrom(src => src.PIN_LOCKOUT))
                    .ForMember(dest => dest.AccountPassword,
                        opts => opts.MapFrom(src => src.ACCOUNT_PASSWORD.Trim()))
                    .ForMember(dest => dest.InvalidPasswordAttempts,
                        opts => opts.MapFrom(src => src.INVALID_PASSWORD_ATTEMPTS))
                    .ForMember(dest => dest.PushNotificationOptOutStatus,
                        opts => opts.MapFrom(src => src.PUSH_NOTIFICATION_OPTOUT_STATUS))
                    .ForMember(dest => dest.SMSAuthorizationCode,
                        opts => opts.MapFrom(src => src.SMS_AUTHORIZATION_CODE.Trim()))
                    .ForMember(dest => dest.SMSOptOutStatus,
                        opts => opts.MapFrom(src => src.SMS_OPTOUT_STATUS))
                    .ForMember(dest => dest.SMSPreEnrollmentMessageCount,
                        opts => opts.MapFrom(src => src.SMS_PRE_ENROLLMENT_MESSAGE_COUNT))
                    .ForMember(dest => dest.EmailOptOutStatus,
                        opts => opts.MapFrom(src => src.EMAIL_OPTOUT_STATUS))
                    .ForMember(dest => dest.TermsAndConditionsAccepted,
                        opts => opts.MapFrom(src => src.TERMS_AND_CONDITIONS_ACCEPTED))
                 
                 .ForMember(dest => dest.BabyClubWD,
                        opts => opts.MapFrom(src => src.BABY_CLUB_WD))
                 .ForMember(dest => dest.BabyClubBL,
                        opts => opts.MapFrom(src => src.BABY_CLUB_BL))
                 .ForMember(dest => dest.BabyClubHvy,
                        opts => opts.MapFrom(src => src.BABY_CLUB_HVY))
                 .ForMember(dest => dest.BabyClubFYM,
                        opts => opts.MapFrom(src => src.BABY_CLUB_FYM))
                 .ForMember(dest => dest.AlcoholPromoOptInStatus,
                        opts => opts.MapFrom(src => src.Alcohol_Promo_Opt_In_Status))
                 .ForMember(dest => dest.ShoppingListTutorialEnabled,
                        opts => opts.MapFrom(src => src.ShoppingList_Tutorial_Enabled))
                 .ForMember(dest => dest.SavingsTutorialEnabled,
                        opts => opts.MapFrom(src => src.Savings_Tutorial_Enabled))
                 .ForMember(dest => dest.RewardsTutorialEnabled,
                        opts => opts.MapFrom(src => src.Rewards_Tutorial_Enabled))
                 .ForMember(dest => dest.EnrollmentOnlineVendor,
                        opts => opts.MapFrom(src => src.Enrollment_Online_Vendor))
                 .ForMember(dest => dest.BoostersIntroEnabled,
                        opts => opts.MapFrom(src => src.Boosters_Intro_Enabled))



                    .ForMember(dest => dest.CustomerCRC,
                        opts => opts.MapFrom(src => src.CustomerCRCs))
                    .ForMember(dest => dest.CustomerAddress,
                        opts => opts.MapFrom(src => src.Addresses))
                    .ForMember(dest => dest.CustomerAlias,
                        opts => opts.MapFrom(src => src.AliasRecords))
                    .ForMember(dest => dest.CustomerChild,
                        opts => opts.MapFrom(src => src.Children))
                .ForMember(dest => dest.CustomerWallet,
                    opts => opts.MapFrom(src => src.Wallet));

               CreateMap<FuzzySearchCustomerResult, CustomerAddress>()
                .IgnoreAllNullMembers()
                    .ForMember(dest => dest.AddressLine1,
                        opts => opts.MapFrom(src => src.STREET_ADDRESS_1.Trim()))
                    .ForMember(dest => dest.AddressLine2,
                        opts => opts.MapFrom(src => src.STREET_ADDRESS_2.Trim()))
                    .ForMember(dest => dest.City,
                        opts => opts.MapFrom(src => src.CITY_NAME.Trim()))
                    .ForMember(dest => dest.State,
                        opts => opts.MapFrom(src => src.STATE_CODE.Trim()))
                    .ForMember(dest => dest.PostalCode,
                        opts => opts.MapFrom(src => src.POSTAL_CODE.Trim()));

               CreateMap<FuzzySearchCustomerResult, MemberAlias>()
                .IgnoreAllNullMembers()
                    .ForMember(dest => dest.MemberId,
                        opts => opts.MapFrom(src => src.MEMBER_ID))
                    .ForMember(dest => dest.AliasNumber,
                        opts => opts.MapFrom(src => src.ALIAS_NUMBER));

               CreateMap<FuzzySearchCustomerResult, CustomerCRC>()
                .IgnoreAllNullMembers()
                    .ForMember(dest => dest.MemberId,
                        opts => opts.MapFrom(src => src.MEMBER_ID))
                    .ForMember(dest => dest.CrcId,
                        opts => opts.MapFrom(src => src.CRC_ID))
                .ForMember(dest => dest.ChainId,
                        opts => opts.MapFrom(src => src.CHAIN_ID));

               CreateMap<FuzzySearchCustomerResult, CustomerV2>()
                .ConvertUsing<FuzzySearchResultToCustomerConverter>();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// An IMappingExpression&lt;TSource,TDestination&gt; extension method that ignore all null
        /// members.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <typeparam name="TSource">      Type of the source. </typeparam>
        /// <typeparam name="TDestination"> Type of the destination. </typeparam>
        /// <param name="expression">   The expression to act on. </param>
        ///
        /// <returns>   An IMappingExpression&lt;TSource,TDestination&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A fuzzy search result to customer converter. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ///
    /// <seealso cref="T:AutoMapper.ITypeConverter{SEG.LoyaltyService.Models.Results.FuzzySearchCustomerResult, SEG.ApiService.Models.Customer}"/>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class FuzzySearchResultToCustomerConverter : ITypeConverter<FuzzySearchCustomerResult, CustomerV2>
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Performs conversion from source to destination type. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="searchResult"> Source object. </param>
        /// <param name="destination">  Destination object. </param>
        /// <param name="context">      Resolution context. </param>
        ///
        /// <returns>   Destination object. </returns>
        ///
        /// <seealso cref="M:AutoMapper.ITypeConverter{SEG.LoyaltyService.Models.Results.FuzzySearchCustomerResult, SEG.ApiService.Models.Customer}.Convert(FuzzySearchCustomerResult,Customer,ResolutionContext)"/>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public CustomerV2 Convert(FuzzySearchCustomerResult searchResult, CustomerV2 destination, ResolutionContext context)
        {

            var formatControl = new System.Globalization.NumberFormatInfo();

            formatControl.NumberDecimalDigits = 0;

            CustomerAddress address = null;
            CustomerCRC crc = null;
            MemberAlias alias = null;

            CustomerV2 customer = new CustomerV2()
            {
                FirstName = searchResult.FIRST_NAME,
                LastName = searchResult.LAST_NAME,
                MemberId = searchResult.MEMBER_ID,
                MobilePhone = searchResult.MOBILE_PHONE,
            };

            if (!string.IsNullOrWhiteSpace(searchResult.STREET_ADDRESS_1) || !string.IsNullOrWhiteSpace(searchResult.STREET_ADDRESS_2) || !string.IsNullOrWhiteSpace(searchResult.CITY_NAME)
                || !string.IsNullOrWhiteSpace(searchResult.STATE_CODE) || !string.IsNullOrWhiteSpace(searchResult.POSTAL_CODE))
            {
                address = new CustomerAddress()
                {
                    AddressLine1 = searchResult.STREET_ADDRESS_1,
                    AddressLine2 = searchResult.STREET_ADDRESS_2,
                    City = searchResult.CITY_NAME,
                    State = searchResult.STATE_CODE,
                    PostalCode = searchResult.POSTAL_CODE
                };
            }


            if (!string.IsNullOrWhiteSpace(searchResult.CHAIN_ID))
            {
                crc = new CustomerCRC()
                {
                    CrcId = searchResult.CRC_ID,
                    ChainId = searchResult.CHAIN_ID
                };
            }

            if (address != null)
            {
                customer.CustomerAddress = new List<CustomerAddress>();
                customer.CustomerAddress.Add(address);
            }


            if (crc != null)
            {
                customer.CustomerCRC = new List<CustomerCRC>();
                customer.CustomerCRC.Add(crc);
            }

            if (alias != null)
            {
                customer.CustomerAlias = new List<MemberAlias>();
                customer.CustomerAlias.Add(alias);
            }
            //}

            return customer;
        }
    }
}
