using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.Classes;
using System.Collections.Generic;
using Capstone.Exceptions;

namespace CapstoneTests.ClassTests
{
    [TestClass]
    public class VendingMachineTests
    {
        // It isn't possible to write a Vending Machine Unit Test because our Vending Machine
        // is entirely dependent on a InventoryFileDAL and TransactionFileLogger. If the inventory file 
        // doesn't exist or fails to write to the file the vending machine won't work.
        // ... but we will learn to solve that later.

        [TestMethod]
        public void FeedMoney_Tests()
        {
            VendingMachine vm = new VendingMachine(null);

            vm.FeedMoney(1);
            vm.FeedMoney(2);
            vm.FeedMoney(5);
            vm.FeedMoney(10);

            Assert.AreEqual(18.0M, vm.CurrentBalance);
        }

        [TestMethod]
        [ExpectedException(typeof(VendingMachineException))]
        public void FeedMoney_Tests_InvalidDollarAmount()
        {
            VendingMachine vm = new VendingMachine(null);

            vm.FeedMoney(15);
        }

        [TestMethod]
        public void GetItemAtSlot_InStock()
        {
            //Arrange
            VendingMachineItem testItem = new ChipItem("Test Chips", 1.25M);
            Dictionary<string, List<VendingMachineItem>> inventory = new Dictionary<string, List<VendingMachineItem>>()
            {
                {"A1", new List<VendingMachineItem>() {testItem} }
            };
            VendingMachine vm = new VendingMachine(inventory);

            //Act
            VendingMachineItem itemAtSlot = vm.GetItemAtSlot("A1");

            //Assert
            Assert.AreEqual(testItem, itemAtSlot);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSlotIDException))]
        public void GetItemAtSlot_InvalidSlot()
        {
            //Arrange
            VendingMachineItem testItem = new ChipItem("Test Chips", 1.25M);
            Dictionary<string, List<VendingMachineItem>> inventory = new Dictionary<string, List<VendingMachineItem>>()
            {
                {"A1", new List<VendingMachineItem>() {testItem} }
            };
            VendingMachine vm = new VendingMachine(inventory);

            VendingMachineItem itemAtSlot = vm.GetItemAtSlot("A2");
        }

        [TestMethod]
        public void GetItemAtSlot_OutOfStock()
        {
            //Arrange
            VendingMachineItem testItem = new ChipItem("Test Chips", 1.25M);
            Dictionary<string, List<VendingMachineItem>> inventory = new Dictionary<string, List<VendingMachineItem>>()
            {
                {"A1", new List<VendingMachineItem>() }
            };
            VendingMachine vm = new VendingMachine(inventory);

            VendingMachineItem itemAtSlot = vm.GetItemAtSlot("A1");

            Assert.AreEqual(null, itemAtSlot);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSlotIDException))]
        public void GetQuantityRemaining_ThrowsInvalidSlot()
        {
            //Arrange
            VendingMachineItem testItem = new ChipItem("Test Chips", 1.25M);
            Dictionary<string, List<VendingMachineItem>> inventory = new Dictionary<string, List<VendingMachineItem>>()
            {
                {"A1", new List<VendingMachineItem>() {testItem} }
            };
            VendingMachine vm = new VendingMachine(inventory);

            int quantityRemaining = vm.GetQuantityRemaining("A2");
        }

        [TestMethod]
        public void GetQuantityRemaining_ReturnsValidQuantity()
        {
            //Arrange
            VendingMachineItem testItem = new ChipItem("Test Chips", 1.25M);
            Dictionary<string, List<VendingMachineItem>> inventory = new Dictionary<string, List<VendingMachineItem>>()
            {
                {"A1", new List<VendingMachineItem>() {testItem} }
            };
            VendingMachine vm = new VendingMachine(inventory);

            int quantityRemaining = vm.GetQuantityRemaining("A1");

            Assert.AreEqual(1, quantityRemaining);
        }

        [TestMethod]
        public void FinishTransactionTests_ReturnChange()
        {
            VendingMachine vm = new VendingMachine(null);

            vm.FeedMoney(1);
            Change returnedChange = vm.ReturnChange();

            Assert.AreEqual(0.0M, vm.CurrentBalance);
            Assert.IsNotNull(returnedChange);
            Assert.AreEqual(4, returnedChange.Quarters);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSlotIDException))]
        public void Purchase_ThrowsInvalidSlot()
        {
            //Arrange
            VendingMachineItem testItem = new ChipItem("Test Chips", 1.25M);
            Dictionary<string, List<VendingMachineItem>> inventory = new Dictionary<string, List<VendingMachineItem>>()
            {
                {"A1", new List<VendingMachineItem>() {testItem} }
            };
            VendingMachine vm = new VendingMachine(inventory);

            vm.Purchase("A2");
        }

        [TestMethod]
        [ExpectedException(typeof(OutOfStockException))]
        public void Purchase_ThrowsOutOfStock()
        {
            //Arrange
            Dictionary<string, List<VendingMachineItem>> inventory = new Dictionary<string, List<VendingMachineItem>>()
            {
                {"A1", new List<VendingMachineItem>()  }
            };
            VendingMachine vm = new VendingMachine(inventory);

            vm.Purchase("A1");
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void Purchase_ThrowsInsufficientFunds()
        {
            VendingMachineItem testItem = new ChipItem("Test Chips", 1.25M);
            Dictionary<string, List<VendingMachineItem>> inventory = new Dictionary<string, List<VendingMachineItem>>()
            {
                {"A1", new List<VendingMachineItem>() {testItem} }
            };
            VendingMachine vm = new VendingMachine(inventory);

            vm.Purchase("A1");
        }

        [TestMethod]
        public void Purchase_ValidPurchase()
        {
            VendingMachineItem testItem = new ChipItem("Test Chips", 1.25M);
            Dictionary<string, List<VendingMachineItem>> inventory = new Dictionary<string, List<VendingMachineItem>>()
            {
                {"A1", new List<VendingMachineItem>() {testItem} }
            };
            VendingMachine vm = new VendingMachine(inventory);

            vm.FeedMoney(2);

            VendingMachineItem purchasedItem = vm.Purchase("A1");

            Assert.IsNotNull(purchasedItem);
            Assert.AreEqual(0, inventory["A1"].Count);
            Assert.AreEqual(0.75M, vm.CurrentBalance);

        }

    }
}
