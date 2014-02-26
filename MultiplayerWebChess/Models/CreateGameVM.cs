using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiplayerWebChess.Models
{
    public enum ColorSelect
    {
        Auto, White, Black
    }

    public class CreateGameVM
    {
        public string Description { get; set; }

        public ColorSelect ColorSelect { get; set; }
    }
}