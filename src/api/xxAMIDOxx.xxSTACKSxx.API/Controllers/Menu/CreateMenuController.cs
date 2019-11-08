﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Menu related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Menu")]
    [ApiController]
    public class CreateMenuController : ApiControllerBase
    {
        ICommandHandler<CreateMenu, Guid> commandHandler;

        public CreateMenuController(ICommandHandler<CreateMenu, Guid> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        /// <summary>
        /// Create a menu
        /// </summary>
        /// <remarks>Adds a menu</remarks>
        /// <param name="body">Menu being created</param>
        /// <response code="201">Resource created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized, Access token is missing or invalid</response>
        /// <response code="403">Forbidden, the user does not have permission to execute this operation</response>
        /// <response code="409">Conflict, an item already exists</response>
        [HttpPost("/v1/menu/")]
        [ProducesResponseType(typeof(ResourceCreatedResponse), 201)]
        public async Task<IActionResult> CreateMenu([Required][FromBody]CreateMenuRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            var id = await commandHandler.HandleAsync(
                new CreateMenu(
                        correlationId: CorrelationId,
                        tenantId: body.TenantId, //Should check if user logged-in owns it
                        name: body.Name,
                        description: body.Description,
                        enabled: body.Enabled
                    )
                );

            return new CreatedAtActionResult(
                    "GetMenu", "GetMenuById", new
                    {
                        id = id
                    }, new ResourceCreatedResponse(id)
            );
        }
    }
}
