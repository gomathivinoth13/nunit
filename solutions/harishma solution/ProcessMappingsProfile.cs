////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	AutoMapperConfig.cs
//
// summary:	Implements the automatic mapper configuration class
////////////////////////////////////////////////////////////////////////////////////////////////////

using AutoMapper;
//using INTF_018 = Plenti.INTF_018;
//using INTF_249 = Plenti.INTF_249;
using SEG.ApiService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEG.ApiService.Models.Attributes;
using SEG.LoyaltyService.Models.Results;
using SEG.ApiService.Models.Enum;

namespace SEG.LoyaltyService.Process.Core
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An automatic mapper configuration. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class ProcessMappingsProfile : Profile
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Registers the membership to customer mappings. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <returns>   A MapperConfiguration. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //public ProcessMappingsProfile()
        //{
        //    CreateMap<INTF_018.MembershipInfoType, Customer>().ConvertUsing<MembershipInfoType018ToCustomerConverter>();
        //}
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A membership information type 018 to customer converter. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class MembershipInfoType018ToCustomerConverter : ITypeConverter<INTF_018.MembershipInfoType, Customer>
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Performs conversion from source to destination type. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="membershipInfoType">   Source object. </param>
        /// <param name="destination">          Destination object. </param>
        /// <param name="context">              Resolution context. </param>
        ///
        /// <returns>   Destination object. </returns>
        ///
        /// <seealso cref="M:AutoMapper.ITypeConverter{Plenti.INTF_018.MembershipInfoType, SEG.ApiService.Models.Customer}.Convert(INTF_018.MembershipInfoType,Customer,ResolutionContext)"/>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Customer Convert(INTF_018.MembershipInfoType membershipInfoType, Customer destination, ResolutionContext context)
        {

            Customer customer = new Customer
            {
                Membership = new Membership()
            };

            if (membershipInfoType.Membership != null)
            {
                if (membershipInfoType.Membership.MembershipIdentifier != null)
                {
                    customer.MemberId = membershipInfoType.Membership.MembershipIdentifier;
                    customer.Membership.MemberId = membershipInfoType.Membership.MembershipIdentifier;
                }

                if (membershipInfoType.Membership.ModificationDate != null && membershipInfoType.Membership.ModificationDate != default(DateTime))
                {
                    customer.Membership.MemberModificationDate = membershipInfoType.Membership.ModificationDate;
                }
                if (membershipInfoType.Membership.ParticipationStatus != null)
                {
                    customer.Membership.MemberStatus = short.Parse(membershipInfoType.Membership.ParticipationStatus);
                }
            }

            if (membershipInfoType.Aliases != null && membershipInfoType.Aliases.Count() > 0)
            {
                customer.Membership.CustAliasRecords = new List<MemberAlias>();
                foreach (INTF_018.AliasInfoType alias in membershipInfoType.Aliases)
                {
                    MemberAlias customerAlias = new MemberAlias();
                    if (alias.AliasType != null)
                    {
                        customerAlias.AliasType = Int32.Parse(alias.AliasType);
                    }
                    //if (alias.EnrollmentCard != null)
                    //{
                    //    customerAlias.EnrollmentCard = alias.EnrollmentCard;
                    //}
                    if (alias.AliasNumber != null)
                    {
                        customerAlias.AliasNumber = alias.AliasNumber;
                    }
                    if (alias.AliasStatus != null)
                    {
                        customerAlias.AliasStatus = GetAliasStatusValue(alias.AliasStatus);
                    }

                    customer.Membership.CustAliasRecords.Add(customerAlias);
                }
            }
            return customer;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets alias status value. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="statusCode">   The status code. </param>
        ///
        /// <returns>   The alias status value. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private short GetAliasStatusValue(string statusCode)
        {
            return short.Parse(statusCode);
        }
    }
}