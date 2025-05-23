using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json.Serialization;

namespace Azure.SQLDB.Samples.DynamicSchema
{    
    [Table("todo_hybrid")]
    public class ToDo
    {        
        public ToDo() {
            Extensions = new ToDoExtension();
        }

        [Key]
        [JsonPropertyName("id")]
        [Column("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        [Column("todo")]
        public string Title { get; set; }

        [JsonPropertyName("completed")]
        [Column("completed", TypeName = "tinyint")]
        public bool Completed { get; set; }
        
        [JsonPropertyName("order")]
        [NotMapped]
        public int Order { 
            get => Extensions.Order;
            set => Extensions.Order = value;
        }

        // [JsonPropertyName("author")]
        // [NotMapped]
        // public string Author { 
        //     get => Extensions.Author;
        //     set => Extensions.Author = value;
        // }

        [JsonPropertyName("url")]
        [NotMapped]
        public string Url { get; set; }            

        [JsonIgnore]
        public ToDoExtension Extensions { get; set; } = null!;        
    }

    public class ToDoExtension {
        [JsonPropertyName("order")]
        public int Order { get; set; }   

        // [JsonPropertyName("author")]
        // public string Author { get; set;}     
    }
}