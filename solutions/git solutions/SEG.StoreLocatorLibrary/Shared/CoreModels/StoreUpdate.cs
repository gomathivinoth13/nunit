using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SEG.StoreLocatorLibrary.Shared
{
    public class StoreUpdate
    {
        [DataMember(Name = "Timings", EmitDefaultValue = false)]
        private List<Timings> timings { get; set; }
        private string storeWorkingHours { get; set; }
        private string storePharmacyHours { get; set; }
        private string storeOpenTime { get; set; }
        private string storeCloseTime { get; set; }
        private string weeklyAdsURL { get; set; }


        [DataMember(Name = "DepartmentList", EmitDefaultValue = false)]
        public string departmentList
        {
            get => _departmentList;
            set
            {
                _departmentList = value;
                _fullText = null;
            }
        }
        string _departmentList = null;

        [MongoDB.Bson.Serialization.Attributes.BsonId]
        [DataMember(Name = "StoreCode", EmitDefaultValue = false)]
        public int StoreCode
        {
            get { return _storeCode; }
            set
            {
                _storeCode = value;
                _fullText = null;
            }
        }
        int _storeCode;

        [DataMember(Name = "ParentStore", EmitDefaultValue = false)]
        public string ParentStore
        {
            get { return _parentStore; }
            set
            {
                _parentStore = value;
                _fullText = null;
            }
        }
        string _parentStore;

        [DataMember(Name = "ChildStore", EmitDefaultValue = false)]
        public int? ChildStore
        {
            get { return _childStore; }
            set
            {
                _childStore = value;
                _fullText = null;
            }
        }
        int? _childStore;


        [DataMember(Name = "ChildStoreRelation", EmitDefaultValue = false)]
        public string ChildStoreRelation
        {
            get { return _childStoreRelation; }
            set
            {
                _childStoreRelation = value;
                _fullText = null;
            }
        }
        string _childStoreRelation;

        [DataMember(Name = "ChildPhone", EmitDefaultValue = false)]
        public string ChildPhone
        {
            get { return _childPhone; }
            set
            {
                _childPhone = value;
                _fullText = null;
            }
        }
        string _childPhone;


        //[DataMember(Name = "StoreSize", EmitDefaultValue = false)]
        public int StoreSize { get { return _storeSize; } set { _storeSize = value; } }
        int _storeSize;

        //[DataMember(Name = "DepartmentFlags", EmitDefaultValue = false)]
        public List<string> DepartmentFlags
        {
            get { return _departmentFlags; }
            set
            {
                _departmentFlags = value;
                _fullText = null;
            }
        }
        List<string> _departmentFlags = new List<string>();

        [DataMember(Name = "StoreName", EmitDefaultValue = false)]
        public string StoreName
        {
            get { return _storeName; }
            set
            {
                _storeName = value;
                _fullText = null;
            }
        }
        string _storeName;
        //[DataMember(Name = "Str_Trt_Desc", EmitDefaultValue = false)]
        //public string Str_Trt_Desc { get; set; }

        [DataMember(Name = "StoreOpenTime", EmitDefaultValue = false)]
        public string StoreOpenTime
        {
            get
            {
                int currentDay = Convert.ToInt16(DateTime.Now.DayOfWeek);
                string openTime = string.Empty;

                if (Timings.Count != 7)
                {
                    for (int time = 0; time < Timings.Count; time++)
                    {
                        if (currentDay == 0 && Timings[time].STR_HRS_DY_NM.Equals("SUN"))
                        {
                            openTime = Timings[time].STR_HRS_OPN_TM;
                        }
                        else if (currentDay == 6 && Timings[time].STR_HRS_DY_NM.Equals("SAT"))
                        {
                            openTime = Timings[time].STR_HRS_OPN_TM;
                        }
                        else if (currentDay > 0 && currentDay < 6 && Timings[time].STR_HRS_DY_NM.Equals("MON - FRI"))
                        {
                            openTime = Timings[time].STR_HRS_OPN_TM;
                        }
                        else if (currentDay > 0 && currentDay < 5 && Timings[time].STR_HRS_DY_NM.Equals("MON - THU"))
                        {
                            openTime = Timings[time].STR_HRS_OPN_TM;
                        }
                    }
                }

                return openTime;
            }
            set
            {
                storeOpenTime = value;
                _fullText = value;
            }
        }

        [DataMember(Name = "StoreCloseTime", EmitDefaultValue = false)]
        public string StoreCloseTime
        {
            get
            {
                int currentDay = Convert.ToInt16(DateTime.Now.DayOfWeek);
                string closeTime = string.Empty;

                if (Timings.Count != 7)
                {
                    for (int time = 0; time < Timings.Count; time++)
                    {
                        if (currentDay == 0 && Timings[time].STR_HRS_DY_NM.Equals("SUN"))
                        {
                            closeTime = Timings[time].STR_HRS_CL_TM;
                        }
                        else if (currentDay == 6 && Timings[time].STR_HRS_DY_NM.Equals("SAT"))
                        {
                            closeTime = Timings[time].STR_HRS_CL_TM;
                        }
                        else if (currentDay > 0 && currentDay < 6 && Timings[time].STR_HRS_DY_NM.Equals("MON - FRI"))
                        {
                            closeTime = Timings[time].STR_HRS_CL_TM;
                        }
                        else if (currentDay > 0 && currentDay < 5 && Timings[time].STR_HRS_DY_NM.Equals("MON - THU"))
                        {
                            closeTime = Timings[time].STR_HRS_CL_TM;
                        }
                    }
                }

                return closeTime;
            }
            set
            {
                storeCloseTime = value;
                _fullText = null;
            }
        }

        [DataMember(Name = "StoreInformation", EmitDefaultValue = false)]
        public string StoreInformation
        {
            get { return _storeInformation; }
            set
            {
                _storeInformation = value;
                _fullText = null;
            }
        }
        string _storeInformation;
        [DataMember(Name = "TimeZone", EmitDefaultValue = false)]
        public string TimeZone
        {
            get { return _timeZone; }
            set
            {
                _timeZone = value;
                _fullText = null;
            }
        }
        string _timeZone;

        [DataMember(Name = "Address", EmitDefaultValue = false)]
        public Address Address
        {
            get { return address; }
            set
            {
                address = value;
                _fullText = null;
            }
        }
        Address address;

        [DataMember(Name = "Location", EmitDefaultValue = false)]
        public Location Location
        {
            get { return location; }
            set
            {
                location = value;
                _fullText = null;
            }
        }
        Location location;


        [DataMember(Name = "Phone", EmitDefaultValue = false)]
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                _fullText = null;
            }
        }
        string phone;

        [DataMember(Name = "Email", EmitDefaultValue = false)]
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                _fullText = null;
            }
        }
        string email;

        [DataMember(Name = "Pharmacy", EmitDefaultValue = false)]
        public Pharmacy Pharmacy
        {
            get { return pharmacy; }
            set
            {
                pharmacy = value;
                _fullText = null;
            }
        }
        Pharmacy pharmacy;

        [DataMember(Name = "MediaLink", EmitDefaultValue = false)]
        public MediaLink MediaLink
        {
            get { return mediaLink; }
            set
            {
                mediaLink = value;
                _fullText = null;
            }
        }
        MediaLink mediaLink;

        [DataMember(Name = "Promotion", EmitDefaultValue = false)]
        public Promotion Promotion
        {
            get { return promotion; }
            set
            {
                promotion = value;
                _fullText = null;
            }
        }
        Promotion promotion;


        [DataMember(Name = "PaginationInfo", EmitDefaultValue = false)]
        public PaginationInfo PaginationInfo
        {
            get { return paginationInfo; }
            set
            {
                paginationInfo = value;
                _fullText = null;
            }
        }
        PaginationInfo paginationInfo;

        public List<Timings> Timings
        {
            get
            {
                List<Timings> storeTimingList = new List<Timings>();
                if (timings != null)
                {
                    if (timings.Count == 7)
                    {
                        if (timings[4].STR_HRS_DY_NM.Equals("Thursday") && timings[5].STR_HRS_DY_NM.Equals("Friday"))
                        {
                            if (timings[4].STR_HRS_OPN_TM.Equals(timings[5].STR_HRS_OPN_TM) && timings[4].STR_HRS_CL_TM.Equals(timings[5].STR_HRS_CL_TM))
                            {
                                Timings storeTimingsMON_FRI = new Timings();
                                storeTimingsMON_FRI.STR_HRS_DY_NM = "MON - FRI";
                                storeTimingsMON_FRI.STR_HRS_OPN_TM = Convert.ToDateTime(timings[1].STR_HRS_OPN_TM).ToString("h:mm tt");
                                storeTimingsMON_FRI.STR_HRS_CL_TM = Convert.ToDateTime(timings[1].STR_HRS_CL_TM).ToString("h:mm tt");
                                storeTimingList.Add(storeTimingsMON_FRI);
                            }
                            else
                            {
                                Timings storeTimingsMON_THU = new Timings();
                                storeTimingsMON_THU.STR_HRS_DY_NM = "MON - THU";
                                storeTimingsMON_THU.STR_HRS_OPN_TM = Convert.ToDateTime(timings[1].STR_HRS_OPN_TM).ToString("h:mm tt");
                                storeTimingsMON_THU.STR_HRS_CL_TM = Convert.ToDateTime(timings[1].STR_HRS_CL_TM).ToString("h:mm tt");
                                storeTimingList.Add(storeTimingsMON_THU);

                                Timings storeTimingsFRI = new Timings();
                                storeTimingsFRI.STR_HRS_DY_NM = "FRI";
                                storeTimingsFRI.STR_HRS_OPN_TM = Convert.ToDateTime(timings[5].STR_HRS_OPN_TM).ToString("h:mm tt");
                                storeTimingsFRI.STR_HRS_CL_TM = Convert.ToDateTime(timings[5].STR_HRS_CL_TM).ToString("h:mm tt");
                                storeTimingList.Add(storeTimingsFRI);
                            }

                        }

                        Timings storeTimingSAT = new Timings();
                        storeTimingSAT.STR_HRS_DY_NM = "SAT";
                        storeTimingSAT.STR_HRS_OPN_TM = Convert.ToDateTime(timings[6].STR_HRS_OPN_TM).ToString("h:mm tt");
                        storeTimingSAT.STR_HRS_CL_TM = Convert.ToDateTime(timings[6].STR_HRS_CL_TM).ToString("h:mm tt");
                        storeTimingList.Add(storeTimingSAT);

                        Timings storeTimingSUN = new Timings();
                        storeTimingSUN.STR_HRS_DY_NM = "SUN";
                        storeTimingSUN.STR_HRS_OPN_TM = Convert.ToDateTime(timings[0].STR_HRS_OPN_TM).ToString("h:mm tt");
                        storeTimingSUN.STR_HRS_CL_TM = Convert.ToDateTime(timings[0].STR_HRS_CL_TM).ToString("h:mm tt");
                        storeTimingList.Add(storeTimingSUN);
                    }
                    else
                    {
                        storeTimingList = timings;
                    }
                }

                return storeTimingList;
            }
            set
            {
                timings = value;
                _fullText = null;
            }
        }


        [DataMember(Name = "WorkingHours", EmitDefaultValue = false)]
        public string WorkingHours
        {
            get
            {
                StringBuilder workingHrs = new StringBuilder();

                if (Timings != null && Timings.Count > 0)
                {
                    if (Timings.Count != 7)
                    {
                        for (int time = 0; time < Timings.Count; time++)
                        {
                            if (Timings.Count - 1 == time)
                            {
                                workingHrs.AppendFormat("{0}: {1} - {2}", Timings[time].STR_HRS_DY_NM, Timings[time].STR_HRS_OPN_TM, Timings[time].STR_HRS_CL_TM);
                            }
                            else
                            {
                                workingHrs.AppendFormat("{0}: {1} - {2}, ", Timings[time].STR_HRS_DY_NM, Timings[time].STR_HRS_OPN_TM, Timings[time].STR_HRS_CL_TM);
                            }
                        }
                    }
                    return workingHrs.ToString();
                }
                else
                {
                    return storeWorkingHours;
                }


            }

            set
            {
                storeWorkingHours = value;
                _fullText = null;
            }
        }

        [DataMember(Name = "PharmacyHours", EmitDefaultValue = false)]
        public string PharmacyHours
        {
            get
            {
                StringBuilder pharmacyHours = new StringBuilder();
                string openTime = string.Empty;
                string closeTime = string.Empty;

                if (Pharmacy != null && Pharmacy.PharmacyHours != null)
                {
                    List<PharmacyHours> pharmacyHour = Pharmacy.PharmacyHours;

                    for (int hr = 0; hr < Pharmacy.PharmacyHours.Count; hr++)
                    {
                        if (hr == pharmacyHour.Count - 1)
                        {
                            pharmacyHours.AppendFormat("{0}: {1} - {2}", pharmacyHour[hr].Day, pharmacyHour[hr].OpenTime, pharmacyHour[hr].CloseTime);
                        }
                        else
                        {
                            pharmacyHours.AppendFormat("{0}: {1} - {2}, ", pharmacyHour[hr].Day, pharmacyHour[hr].OpenTime, pharmacyHour[hr].CloseTime);
                        }
                    }

                    return pharmacyHours.ToString();
                }
                else
                {
                    return storePharmacyHours;
                }

            }

            set
            {
                storePharmacyHours = value;
                _fullText = null;
            }
        }

        [DataMember(Name = "TransactionId", EmitDefaultValue = false)]
        public string TransactionId { get; set; }


        [DataMember(Name = "Distance", EmitDefaultValue = true)]
        public double Distance { get; set; }

        [DataMember(Name = "WeeklyAds", EmitDefaultValue = false)]
        public string WeeklyAds
        {
            get { return weeklyAds; }
            set
            {
                weeklyAds = value;
                _fullText = null;
            }
        }
        string weeklyAds;


        [DataMember(Name = "OnlineGrocery", EmitDefaultValue = true)]
        public Boolean OnlineGrocery { get; set; }

        [DataMember(Name = "Chain_ID", EmitDefaultValue = false)]
        public string Chain_ID { get; set; }

        [DataMember(Name = "StoreBannerTypDesc", EmitDefaultValue = false)]
        public string StoreBannerTypDesc
        {
            get { return storeBannerTypeDesc; }
            set
            {
                storeBannerTypeDesc = value;
                _fullText = null;
            }
        }
        string storeBannerTypeDesc;

        [DataMember(Name = "StoreOpenDate", EmitDefaultValue = false)]
        public DateTime StoreOpenDate
        {
            get { return storeOpenDate; }
            set
            {
                storeOpenDate = value;
                _fullText = null;
            }
        }
        DateTime storeOpenDate;

        [DataMember(Name = "StoreCloseDate", EmitDefaultValue = false)]
        public DateTime StoreCloseDate
        {
            get { return storeCloseDate; }
            set
            {
                storeCloseDate = value;
                _fullText = null;
            }
        }
        DateTime storeCloseDate;

        [DataMember(Name = "IsFutureStoreFlag", EmitDefaultValue = false)]
        public bool IsFutureStoreFlag
        {
            get { return isFutureStoreFlag; }
            set
            {
                isFutureStoreFlag = value;
                _fullText = null;
            }
        }
        bool isFutureStoreFlag;

        string _fullText = null;
        public string FullText
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_fullText))
                {

                    _fullText = this.ToString();
                }
                return _fullText;
            }
            set { _fullText = value; }
        }


        // Zero references to both but required by CosmosDB Model
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }


        public override string ToString()
        {
            // Zero reference TODO: Remove override after testing
            throw new NotImplementedException("Store.ToString()");

            //StringBuilder sb = new StringBuilder();
            //sb.AppendFormat("{0} ", departmentList);
            //sb.AppendFormat("{0} ", StoreCode);
            //sb.AppendFormat("{0} ", ParentStore);
            //sb.AppendFormat("{0} ", ChildStore);
            //sb.AppendFormat("{0} ", ChildStoreRelation);
            //sb.AppendFormat("{0} ", ChildPhone);
            //sb.AppendFormat("{0} ", string.Join(" ", DepartmentFlags));
            //sb.AppendFormat("{0} ", StoreName);
            //sb.AppendFormat("{0} ", StoreOpenTime);
            //sb.AppendFormat("{0} ", StoreCloseTime);
            //sb.AppendFormat("{0} ", StoreInformation);
            //sb.AppendFormat("{0} ", TimeZone);
            //sb.AppendFormat("{0} ", Address);
            //sb.AppendFormat("{0} ", Location);
            //sb.AppendFormat("{0} ", Phone);
            //sb.AppendFormat("{0} ", Email);
            //sb.AppendFormat("{0} ", WorkingHours);
            //sb.AppendFormat("{0} ", PharmacyHours);
            //sb.AppendFormat("{0} ", StoreBannerTypDesc);
            //sb.AppendFormat("{0} ", OnlineGrocery);
            //sb.AppendFormat("{0} ", StoreOpenTime);
            //sb.AppendFormat("{0} ", StoreCloseTime);
            //sb.AppendFormat("{0} ", IsFutureStoreFlag);

            //return base.ToString();
        }
    }
}
