using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWash.Models;

public class Admin
{
    [Key]
    public int AdminId{get;set;}
    [Required]
    public string AdminName{get;set;}
    [Required]
    public string Password{get;set;}
    [Required]

    public string Email{get;set;}


}
