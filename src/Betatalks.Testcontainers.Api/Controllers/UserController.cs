using AutoMapper;
using Betatalks.Testcontainers.Api.TransferObjects;
using Betatalks.Testcontainers.Core.Exceptions;
using Betatalks.Testcontainers.Core.Interfaces.Services;
using Betatalks.Testcontainers.Core.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Betatalks.Testcontainers.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IMapper _mapper;

    public UserController(IUserService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /**
        <summary>
        Retrieves Users present in the system.
        </summary>
        <response code="200">Returns Users present in the system.</response>
        <response code="400">The request went wrong.</response>
    */
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var usersResult = await _service.GetUsersAsync(cancellationToken).ConfigureAwait(false);
        if (usersResult.Failed)
        {
            return BadRequest();
        }
        return Ok(_mapper.Map<IEnumerable<UserDto>>(usersResult.Value));
    }

    /**
        <summary>
        Retrieve User based on provided Id.
        </summary>
        <response code="200">Returns User present in the system.</response>
        <response code="400">The request went wrong.</response>
        <response code="404">No User with the provided Id could be found.</response>
    */
    [HttpGet("{id}", Name = nameof(GetUserByIdAsync))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var usersResult = await _service.GetUserByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (usersResult.Failed && usersResult.Exception is NotFoundException)
        {
            return NotFound();
        }
        if (usersResult.Failed)
        {
            return BadRequest();
        }
        return Ok(_mapper.Map<UserDto>(usersResult.Value));
    }

    /**
        <summary>
        Add new User.
        </summary>
        <response code="201">Returns route to newly created User.</response>
        <response code="400">The user provided a malformed creation request.</response>
    */
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddUserAsync(UserUpsertDto userUpsertDto, CancellationToken cancellationToken = default)
    {
        var userResult = await _service.AddUserAsync(_mapper.Map<UpsertUserRequest>(userUpsertDto), cancellationToken).ConfigureAwait(false);
        if (userResult.Failed && userResult.Exception is ArgumentException or FormatException)
        {
            return BadRequest(userResult.Exception.Message);
        }
        if (userResult.Failed)
        {
            return BadRequest();
        }
        return CreatedAtRoute(nameof(GetUserByIdAsync), new { id = userResult.Value?.Id }, userResult.Value);
    }
}
