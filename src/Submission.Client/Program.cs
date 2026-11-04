using System.Net.Http.Json;

var apiBaseUrl = args.Length > 0 ? args[0] : "http://localhost:5000";
using var client = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };

try {
    // 1. Lấy danh sách bài tập từ API
    var assignments = await client.GetFromJsonAsync<List<Assignment>>("/api/assignments");
    Console.WriteLine("Danh sach bai tap:");
    foreach (var a in assignments ?? []) {
        Console.WriteLine($"   - {a.Id}: {a.Title} ({a.Description})");
    }

    // 2. Gửi bài nộp mới với mã sinh viên dtc245200226
    var newSubmission = new CreateSubmissionRequest(
        AssignmentId: 1,
        StudentCode: "dtc245200226",
        RepositoryUrl: "https://github.com/dtc245200226/EduSubmit-dtc245200226"
    );

    var response = await client.PostAsJsonAsync("/api/submissions", newSubmission);
    response.EnsureSuccessStatusCode();

    var result = await response.Content.ReadFromJsonAsync<SubmissionResult>();
    if (result != null) {
        Console.WriteLine($"Da nop bai. Ma luot nop: {result.Id}   Trang thai: {result.Status}");

        // 3. Bài mở rộng: Gọi lại endpoint GET theo ID để đọc thông tin
        var checkResponse = await client.GetAsync($"/api/submissions/{result.Id}");
        if (checkResponse.IsSuccessStatusCode) {
            var fetchedResult = await checkResponse.Content.ReadFromJsonAsync<SubmissionResult>();
            Console.WriteLine($"[Kiem tra lai] Doc thanh cong luot nop ID: {fetchedResult?.Id} - Trang thai: {fetchedResult?.Status}");
        }
    }
}
catch (Exception ex) {
    Console.WriteLine($"Loi khi giao tiep voi API: {ex.Message}");
}

record Assignment(int Id, string Title, string Description);
record CreateSubmissionRequest(int AssignmentId, string StudentCode, string RepositoryUrl);
record SubmissionResult(Guid Id, int AssignmentId, string StudentCode, string Status, DateTimeOffset ReceivedAt);