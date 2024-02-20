using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static HomeBankingMinHub.Services.ClientService;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var clientsDTO = _clientService.GetAllClients();
                return Ok(clientsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var clientDTO = _clientService.GetClientById(id); 
                return Ok(clientDTO);
            }
            catch (ClientServiceException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClientRegisterDTO client)
        {
            try
            {
                Client newClient = _clientService.CreateClient(client);
                return Created("", newClient);
            }
            catch (ClientServiceException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                
                if (string.IsNullOrEmpty(email))
                {
                    return StatusCode(403, "No hay clientes logeados");
                }
                ClientDTO clientDTO = _clientService.GetCurrentClient(email);   
                return Ok(clientDTO);
            }
            catch (ClientServiceException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
