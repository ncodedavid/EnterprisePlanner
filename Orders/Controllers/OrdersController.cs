﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Orders.Entities;
using Orders.Repositories;
using Orders.Models;

namespace Orders.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _repo;

        public OrdersController(IOrderRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            try
            {
                var ordersFromRepo = _repo.GetOrders();
                var orders = Mapper.Map<IEnumerable<OrderDto>>(ordersFromRepo);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult GetOrder([FromRoute]int id)
        {
            try
            {
                if (!_repo.OrderExists(id)) { return NotFound(); }
                var order = Mapper.Map<OrderDto>(_repo.GetOrder(id));
                return Ok(order);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody]OrderToCreateDto orderToCreate)
        {
            try
            {
                if (orderToCreate == null) return BadRequest();
                var orderEntity = Mapper.Map<Order>(orderToCreate);
                _repo.AddOrder(orderEntity);
                if (!_repo.Save())
                {
                    throw new Exception("Error creating order.");
                }
                var orderToReturn = Mapper.Map<OrderDto>(orderEntity);
                return CreatedAtRoute("GetOrder", new { id = orderToReturn.Id }, orderToReturn);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody]OrderToUpdateDto orderToUpdate)
        {
            if (orderToUpdate == null) return BadRequest();

            var orderFromRepo = _repo.GetOrder(id);
            if (orderFromRepo == null) return NotFound();
            try
            {
                Mapper.Map(orderToUpdate, orderFromRepo);
                _repo.UpdateOrder(orderFromRepo);
                _repo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var orderFromRepo = _repo.GetOrder(id);
            if (orderFromRepo == null) return NotFound();
            _repo.DeleteOrder(orderFromRepo);
            if (!_repo.Save())
            {
                throw new Exception($"Error deleting order {id}.");
            }

            return NoContent();
        }
    }
}