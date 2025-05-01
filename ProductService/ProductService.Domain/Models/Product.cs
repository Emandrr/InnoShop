using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Models
{
    public class Product
    {
        public Product(Guid Id,string Name, string Description,int Price, string UserId,DateTime CreatedAt)
        {
            this.Id = Id;
            this.Name = Name;
            this.UserId = UserId;
            this.Price = Price;
            this.Available = true;
            this.Description = Description;
            this.CreatedAt = CreatedAt.ToUniversalTime();
        }
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public int Price { get; private set; }
        public bool Available { get;set; }
        public string UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public static Product Create(Guid Id,string productName,string Description, int Price, string UserId,DateTime CreatedAt )
        {
            return new Product(Id,productName,Description,Price,UserId,CreatedAt.ToUniversalTime());
        }
    }
}
