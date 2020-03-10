using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public enum SeriesType { Input = 0, Processing = 1, Export = 2 };
    public class StatSeries
    {       
        public int Data { get; set; } = 0;
        public int Value { get; set; } = 0;
    }
}