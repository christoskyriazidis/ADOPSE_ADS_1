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

        public Product(string title, int quantity, int price)
        {
            this.Title = title;
            this.Quantity = quantity;
            this.Price = price;
        }

        public Product()
        {

        }
        public int Price { get => price; set => price = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public string Title { get => title; set => title = value; }
    }
}
