using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Databases
{
    public class Product
    {
        private string title;
        private int quantity;
        private int price;
        private int size;
        private string description;

        public Product(string title, int quantity, int price, int size, string description)
        {
            this.title = title;
            this.quantity = quantity;
            this.price = price;
            this.Size = size;
            this.Description = description;
        }

        public Product()
        {

        }
        public int Price { get => price; set => price = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public string Title { get => title; set => title = value; }
        public int Size { get => size; set => size = value; }
        public string Description { get => description; set => description = value; }
    }
}
