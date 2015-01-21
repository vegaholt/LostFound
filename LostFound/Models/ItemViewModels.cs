using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LostFound.Models
{

    public class MyItemsViewModel
    {
        public List<Item> LostItems { get; set; }
        public List<Item> FoundItems { get; set; }
    }

    public class NewItemViewModel 
    {
        public Item Item { get; set; }

        [Display(Name = "Dato funnet")]
        public string DateFound { get; set; }

        [Display(Name = "Dato mistet, fra")]
        public string DateLostFrom { get; set; }

        [Display(Name = "Dato mistet, til")]
        public string DateLostTo { get; set; }

        public List<Category> Categories { get; set; }

        [Display(Name = "Bilde")]
        public HttpPostedFileBase Image { get; set; }
    }

    public class IndexItemViewModel
    {
        public List<Item> Items { get; set; }

        public List<Category> Categories { get; set; }

        public List<County> Counties { get; set; }

        public bool IsLost { get; set; }

        public string SearchString { get; set; }

        public string FromDate { get; set; }

        public List<string> SelectedCategories { get; set; }

        public List<string> SelectedCounties { get; set; }

        public int Hits { get; set; }
    }
}