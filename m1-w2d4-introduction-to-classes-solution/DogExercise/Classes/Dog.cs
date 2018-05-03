using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechElevator.Classes
{
    /**
    * This class represents a dog that can sleep, wake, and make a sound, so it's pretty much like most dogs,
    * with a few essential methods missing.
    */
    public class Dog
    {
                
        public bool IsSleeping { get; private set; }
        
        public Dog()
        {
            this.IsSleeping = false;
        }        
        
        public string MakeSound()
        {
            if (this.IsSleeping)
            {
                return "Zzzzz...";
            }
            else
            {
                return "woof!";
            }
        }

        
        public void Sleep()
        {
            this.IsSleeping = true;
        }

        
        public void WakeUp()
        {
            this.IsSleeping = false;
        }


    }
}
