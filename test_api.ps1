Write-Host "=== TEST 1: GET ALL TRAINERS ==="
$trainers = Invoke-RestMethod -Uri "http://localhost:5089/api/trainers" -Method Get
Write-Host "Total trainers count: $($trainers.Count)"

Write-Host "`n=== TEST 2: GET TRAINERS WITH QUERY FILTER (Yoga) ==="
$yogaTrainers = Invoke-RestMethod -Uri "http://localhost:5089/api/trainers?specialty=Yoga" -Method Get
Write-Host "Found $($yogaTrainers.Count) Yoga trainer: $($yogaTrainers[0].fullName)"

Write-Host "`n=== TEST 3: GET TRAINER BY ID (200 OK & 404 NOT FOUND) ==="
$t1 = Invoke-RestMethod -Uri "http://localhost:5089/api/trainers/1" -Method Get
Write-Host "Trainer 1: $($t1.fullName) - Specialty: $($t1.specialty)"

try {
    Invoke-RestMethod -Uri "http://localhost:5089/api/trainers/999" -Method Get
} catch {
    Write-Host "Trainer 999 returned HTTP Status: $($_.Exception.Response.StatusCode.value__)"
}

Write-Host "`n=== TEST 4: TEST VALIDATION (POST INVALID DATA) ==="
$invalidBody = @'
{
    "username": "pt",
    "password": "123",
    "fullName": "",
    "email": "not-an-email",
    "phone": "abc",
    "dateOfBirth": "2020-01-01",
    "specialty": "",
    "maxMembers": 7
}
'@

try {
    Invoke-RestMethod -Uri "http://localhost:5089/api/trainers" -Method Post -ContentType "application/json" -Body $invalidBody
} catch {
    Write-Host "Validation failed as expected! Status: $($_.Exception.Response.StatusCode.value__)"
    Write-Host "Response Body: $($_.ErrorDetails.Message)"
}

Write-Host "`n=== TEST 5: CREATE NEW TRAINER (POST VALID DATA - 201 CREATED) ==="
$validTrainer = @'
{
    "username": "pt_hoangnam",
    "password": "Password123",
    "fullName": "Lê Hoàng Nam",
    "email": "nam.le@fitnessclub.vn",
    "phone": "0988776655",
    "gender": "Male",
    "dateOfBirth": "1997-06-15",
    "specialty": "Fitness",
    "experienceYears": 3,
    "certifications": "ACE Certified Personal Trainer",
    "hourlyRate": 350000,
    "maxMembers": 8
}
'@

$newTrainer = Invoke-RestMethod -Uri "http://localhost:5089/api/trainers" -Method Post -ContentType "application/json" -Body $validTrainer
Write-Host "Created new trainer with Id: $($newTrainer.id), Name: $($newTrainer.fullName)"

Write-Host "`n=== TEST 6: NESTED ROUTE (GET TRAINER ASSIGNED MEMBERS) ==="
$members = Invoke-RestMethod -Uri "http://localhost:5089/api/trainers/1/members" -Method Get
Write-Host "Trainer 1 assigned members count: $($members.Count)"
foreach ($m in $members) {
    Write-Host " - Member: $($m.fullName) ($($m.username)) - Package: $($m.membershipPackage)"
}

Write-Host "`n=== TEST 7: GET ALL STAFF & TOGGLE DUTY ==="
$staffs = Invoke-RestMethod -Uri "http://localhost:5089/api/staffs" -Method Get
Write-Host "Total staff count: $($staffs.Count)"
$staff1 = $staffs[0]
Write-Host "Staff 1: $($staff1.fullName), Shift: $($staff1.workShift), OnDuty: $($staff1.isOnDuty)"

$toggleResult = Invoke-RestMethod -Uri "http://localhost:5089/api/staffs/$($staff1.id)/toggle-duty" -Method Patch
Write-Host "Toggled duty result: $($toggleResult.message) - Current OnDuty: $($toggleResult.isOnDuty)"

Write-Host "`n=== TEST 8: VERIFY EXISTING USERS API STILL WORKS (100% UNCHANGED) ==="
$users = Invoke-RestMethod -Uri "http://localhost:5089/api/users" -Method Get
Write-Host "Total system users: $($users.totalSystemUsers)"
