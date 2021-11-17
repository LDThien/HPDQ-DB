using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPDQ_Project.Models
{
    public class Employees_API
    {
        public class Rootobject
        {
            public Timesheets[] Property { get; set; }
        }

  //   {
  //  "SEQ": "1",
  //  "PK": "213536499",
  //  "THR_EMP_PK": "5185",
  //  "EMP_ID": "HPDQ00001",
  //  "FULL_NAME": "Mai Văn Hà",
  //  "PHONE_NUMBER": "0903276968",
  //  "WORK_DT": "20201101",
  //  "DAY_TYPE": "SUN",
  //  "THR_SHIFT_PK": "51",
  //  "SHIFT_NM": "6",
  //  "ORG_ID": "1051",
  //  "ORG_NM": "Ban giám đốc",
  //  "DATE_IN": null,
  //  "TIME_IN": null,
  //  "DATE_OUT": null,
  //  "TIME_OUT": null,
  //  "WT": "0",
  //  "REASON_CODE": null,
  //  "REASON_NAME": null,
  //  "STATUS": 1.0,
  //  "MOD_YN": null,
  //  "MOD_DATE": "02/11/2020 09:00:04",
  //  "CRT_DATE": "02/11/2020 09:00:04"
  //}

    public class Timesheets
        {
            public string SEQ { get; set; }
            public string PK { get; set; }
            public string THR_EMP_PK { get; set; }
            public string EMP_ID { get; set; }
            public string FULL_NAME { get; set; }
            public string PHONE_NUMBER { get; set; }
            public string WORK_DT { get; set; }
            public string DAY_TYPE { get; set; }
            public string THR_SHIFT_PK { get; set; }
            public string ORG_ID { get; set; }
            public string ORG_NM { get; set; }
            public string DATE_IN { get; set; }
            public string TIME_IN { get; set; }
            public string DATE_OUT { get; set; }
            public string TIME_OUT { get; set; }
        }
        public class E_API
        {
            public string MaNV { get; set; }
            public string TenNV { get; set; }
            public Nullable<int> MaPB { get; set; }
            public string TenPB { get; set; }
            public Nullable<int> MaCV { get; set; }
            public string TenCV { get; set; }
            public Nullable<int> SoMayLe { get; set; }
            public string Email { get; set; }



        }

    }
}