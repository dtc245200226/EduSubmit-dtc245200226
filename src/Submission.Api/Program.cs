var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var assignments = new[] {
    new Assignment(1, "Bai tap 1", "Tao API dau tien"),
    new Assignment(2, "Bai tap 2", "Goi API tu client")
};

// Danh sách lưu trữ các bài nộp (phục vụ bài mở rộng)
var submissions = new List<SubmissionResult>();

app.MapGet("/", () => Results.Ok(new {
    service = "Submission.Api",
    status = "running"
}));

app.MapGet("/api/assignments", () => Results.Ok(assignments));

app.MapPost("/api/submissions", (CreateSubmissionRequest request) => {
    if (request.AssignmentId <= 0 ||
        string.IsNullOrWhiteSpace(request.StudentCode) ||
        string.IsNullOrWhiteSpace(request.RepositoryUrl)) {
        return Results.BadRequest(new { message = "Du lieu nop bai khong hop le" });
    }

    var submission = new SubmissionResult(
        Guid.NewGuid(),
        request.AssignmentId,
        request.StudentCode,
        "received",
        DateTimeOffset.UtcNow);

    submissions.Add(submission);
    return Results.Created($"/api/submissions/{submission.Id}", submission);
});

// Bài mở rộng: Endpoint GET lấy thông tin bài nộp theo ID (Guid)
app.MapGet("/api/submissions/{id:guid}", (Guid id) => {
    var sub = submissions.FirstOrDefault(s => s.Id == id);
    return sub is not null ? Results.Ok(sub) : Results.NotFound(new { message = "Khong tim thay luot nop" });
});

app.Run();

record Assignment(int Id, string Title, string Description);
record CreateSubmissionRequest(int AssignmentId, string StudentCode, string RepositoryUrl);
record SubmissionResult(Guid Id, int AssignmentId, string StudentCode, string Status, DateTimeOffset ReceivedAt);