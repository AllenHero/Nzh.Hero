﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Model
{
    public class sys_dictionary
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long id { get; set; }
        public string dic_name { get; set; }
        public string dic_value { get; set; }
        public long parent_id { get; set; }
        public string dic_code { get; set; }
        public int sort_num { get; set; }
        public string create_person { get; set; }
        public DateTime create_time { get; set; }
        public string remark { get; set; }
    }
}
