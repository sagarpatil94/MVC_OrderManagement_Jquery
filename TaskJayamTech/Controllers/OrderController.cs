using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text;
using TaskJayamTech.CommonServices;
using TaskJayamTech.DataContext;
using TaskJayamTech.Models;
using TaskJayamTech.Repository.OrderService;

namespace TaskJayamTech.Controllers
{  
    /// <summary>
    /// Order 
    /// </summary>
    public class OrderController : Controller
    {
        private readonly IOrderRepository _repository;
        public OrderController(IOrderRepository repository)
        {
          _repository = repository;
        }
        /// <summary>
        /// Get All Order List
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
         
            var result = await _repository.GetOrder();
            return View(result);
        }
        public async Task<IActionResult> Create(int id)
        {
            var result = await _repository.GetMaster();
            ViewBag.OrderNumber = GetLatestOrderNumber();
            ViewBag.CustomerList = new SelectList(result.customer, "Id", "Name");
            ViewBag.ItemList = new SelectList(result.items, "Id", "Name");
            if (id >0)
            {
                var data =await _repository.GetByIdOrder(id);
              
                return View(data);  
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                if (order.Id > 0)
                {
                    var Isupdate = await _repository.EditOrder(order);
                    if (Isupdate)
                    {
                        TempData["AlertMsg"] = AlertService.ShowAlert(Alerts.Success, "Order Successfully Updated ...!");
                        return RedirectToAction("Index");
                    }
                }
                var result = await _repository.AddOrder(order);
                if (result)
                {
                    TempData["AlertMsg"] = AlertService.ShowAlert(Alerts.Success, "Order Successfully place..!");
                    return RedirectToAction("Index");
                }
                else
                {
                  return BadRequest(order);  
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> GetItemById(int itemId)
        {
            if (itemId > 0)
            {
                var result = await _repository.GetMaster();
                if (result == null) return NotFound();
                var data = result.items.Where(x => x.Id == itemId).ToList();
                return new JsonResult(data);
            }
            return NotFound();  
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id> 0)
            {
                var isDelete = await _repository.DeleteOrder(id);
                if (isDelete)
                {
                    TempData["AlertMsg"] = AlertService.ShowAlert(Alerts.Danger, "Oredr Deleted ..!");
                    return RedirectToAction("Index");
                }
            }
            return BadRequest();
        }
        private string GetLatestOrderNumber()
        {
          var str =   string.Format("ORD{0:000}", 00 + 1);

          /* StringBuilder sb = new StringBuilder();
            sb.Append("ORD");
            Random rnd = new Random();
            for (int i = 0; i < 3; i++)
            {
                int value = rnd.Next();
                sb.Append(value.ToString());
            }*/
         
               
            return str.ToString();
        }
    }
}
