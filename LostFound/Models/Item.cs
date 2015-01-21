using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace LostFound.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        [Display(Name = "Tittel")]
        public string Name { get; set; }

        [Display(Name = "Beskrivelse")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Kategori")]
        public virtual Category Category { get; set; }

        [Display(Name = "Bilde")]
        public string ImgUrl { get; set; }

        [Display(Name = "Finnerlønn (kr)")]
        public double Reward { get; set; }

        [Display(Name = "Eierskapsbevis")]
        public string Claim { get; set; }

        public string Lat { get; set; }

        public string Lon { get; set; }

        [Display(Name = "Fylke")]
        public virtual County County { get; set; }

        [Display(Name = "Adresse")]
        public string Adress { get; set; }

        [Display(Name = "Dato funnet")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FoundDate { get; set; }

        [Display(Name = "Dato tapt, fra")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime LostDateFrom { get; set; }

        [Display(Name = "Dato tapt, til")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime LostDateTo { get; set; }

        [Required]
        public bool Lost { get; set; }

    }
}