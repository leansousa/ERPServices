using ERPServices.CashFlow.API.Data.ValueObjects;
using ERPServices.CashFlow.API.RabbitMQSender;
using ERPServices.CashFlow.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPServices.CashFlow.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles ="ADMIN")]
    public class CashFlowController : ControllerBase
    {
        private readonly ICashFlowRepository _repository;
        private readonly IRabbitMQMessageSender _rabbitMQMessageSender;
        public CashFlowController(ICashFlowRepository repository, IRabbitMQMessageSender rabbitMQMessageSender)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CashFlowVO>>> FindAll()
        {
            var result = await _repository.FindAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CashFlowVO>> FindById(long id)
        {
            var result = await _repository.FindbyId(id);
            if (result.Id <= 0) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CashFlowVO>> Create(CashFlowVO vo)
        {
            if (vo == null) return BadRequest();
            var result = await _repository.Create(vo);

            var message = new CashFlowMessageVO
            {
                Id = vo.Id,
                Value = vo.Value,
                Description = vo.Description,
                Type = vo.Type,
                Date = vo.Date,
                Operation = "C"
            };

            _rabbitMQMessageSender.SendMessage(message);

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<CashFlowVO>> Update(CashFlowVO vo)
        {
            if (vo == null) return BadRequest();

            var item = await _repository.FindbyId(vo.Id);

            if (item.Id <= 0) return BadRequest();

            var message = new CashFlowMessageVO
            {
                Id = vo.Id,
                Value = vo.Value,
                Description = vo.Description,
                Type = vo.Type,
                Date = vo.Date,
                Operation = "U",
                ValueOld = item.Value
            };

            _rabbitMQMessageSender.SendMessage(message);

            var result = await _repository.Update(vo);


            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var item = await _repository.FindbyId(id);

            if (item.Id <= 0) return BadRequest();

            var message = new CashFlowMessageVO
            {
                Id = item.Id,
                Value = item.Value,
                Description = item.Description,
                Type = item.Type,
                Date = item.Date,
                Operation = "D",
                ValueOld = item.Value
            };

            _rabbitMQMessageSender.SendMessage(message);


            var status = await _repository.Delete(id);
            if (!status) return BadRequest();


            return Ok(status);
        }
    }
}
