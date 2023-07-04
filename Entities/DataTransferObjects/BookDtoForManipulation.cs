﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public abstract record BookDtoForManipulation
    {
        [Required(ErrorMessage = "Title is required field")]
        [MinLength(1,ErrorMessage = "Title must be least 2 characters")]
        [MaxLength(50, ErrorMessage = "Title must be most 50 characters")]
        public String Title { get; init; }
        [Required(ErrorMessage = "Price is required field")]
        [Range(10, 1000, ErrorMessage = "Price must be between 10-1000")]
        public decimal Price { get; init; }
    }
}
