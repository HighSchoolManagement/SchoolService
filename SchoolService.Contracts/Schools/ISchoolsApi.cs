using Refit;

namespace SchoolService.Contracts.Schools;

/// <summary>
/// Refit client for the SchoolService HTTP API (route prefix: /api/schools).
/// Base URL is supplied by the consuming service when it registers the client.
/// </summary>
public interface ISchoolsApi
{
    /// <summary>GET /api/schools?page=&amp;pageSize= — paged list of schools.</summary>
    [Get("/api/schools")]
    Task<ApiResponse<PagedResult<GetSchoolsResponse>>> GetSchoolsAsync(
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    /// <summary>GET /api/schools/{id} — 404 when the school does not exist.</summary>
    [Get("/api/schools/{id}")]
    Task<ApiResponse<GetSchoolByIdResponse>> GetSchoolByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    /// <summary>POST /api/schools — 201 on success, 409 when the school code already exists.</summary>
    [Post("/api/schools")]
    Task<ApiResponse<CreateSchoolResponse>> CreateSchoolAsync(
        [Body] CreateSchoolRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>PATCH /api/schools/{id} — partial update, 204 on success, 404 when not found.</summary>
    [Patch("/api/schools/{id}")]
    Task<IApiResponse> UpdateSchoolAsync(
        int id,
        [Body] UpdateSchoolRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>DELETE /api/schools/{id} — soft delete, 204 on success, 404 / 409 otherwise.</summary>
    [Delete("/api/schools/{id}")]
    Task<IApiResponse> DeleteSchoolAsync(
        int id,
        CancellationToken cancellationToken = default);
}
