using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace YueMiaoNotifier
{
    public partial class Protocol<T>
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("ok")]
        public bool Ok { get; set; }
    }


    public partial class VaccineDetailResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("intro")]
        public string Intro { get; set; }

        [JsonProperty("describtion")]
        public string Describtion { get; set; }

        [JsonProperty("startTime")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("startMilliscond")]
        public long StartMilliscond { get; set; }

        [JsonProperty("now")]
        public long Now { get; set; }

        [JsonProperty("workTimeStart")]
        public string WorkTimeStart { get; set; }

        [JsonProperty("workTimeEnd")]
        public string WorkTimeEnd { get; set; }
    }

    public partial class VaccineListingItemResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("intro")]
        public string Intro { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("isSeckill")]
        public long IsSeckill { get; set; }
    }

    public partial class DepartmentListRespnose
    {
        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("pageNumber")]
        public long PageNumber { get; set; }

        [JsonProperty("pageListSize")]
        public long PageListSize { get; set; }

        [JsonProperty("pageNumList")]
        public long[] PageNumList { get; set; }

        [JsonProperty("rows")]
        public Row[] Rows { get; set; }

        [JsonProperty("pages")]
        public long Pages { get; set; }
    }

    public partial class Row
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imgUrl")]
        public string ImgUrl { get; set; }

        [JsonProperty("regionCode")]
        public string RegionCode { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("tel")]
        public string Tel { get; set; }

        [JsonProperty("isOpen")]
        public long IsOpen { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("worktimeDesc")]
        public string WorktimeDesc { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("isSeckill")]
        public string IsSeckill { get; set; }

        [JsonProperty("depaCodes")]
        public string[] DepaCodes { get; set; }

        [JsonProperty("vaccines")]
        public Vaccine[] Vaccines { get; set; }
    }

    public partial class Vaccine
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("intro")]
        public string Intro { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("isSeckill")]
        public long IsSeckill { get; set; }
    }
}
