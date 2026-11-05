Act as a Senior QA and Backend Engineer. Based on the 7 critical shortfalls you identified, I need concrete test cases to simulate and prove that these errors can actually occur while executing the jobs. 

For each of the following 7 issues, provide:
1. The exact scenario setup (e.g., mock data payloads, system state, or database constraints).
2. A step-by-step simulation/execution workflow.
3. The expected failure outcome or assertion to look for.

Please generate replication steps or unit/integration test structures for these specific items:

1. No DbUpdateException Handling: Provide a test case that inserts a duplicate key or violates a foreign key constraint to prove silent data loss occurs without a thrown or caught exception.
2. CancellationToken Misclassified: Simulate a job shutdown/cancellation signal mid-execution and show how the system erroneously marks it as a permanent job failure.
3. RunId Null Not Validated: Pass a null or missing RunId parameter into the job execution pipeline to trigger and capture the NullReferenceException crash.
4. No Batch Size Limit: Provide a simulation using a massive payload/batch that exceeds default payload constraints, forcing a silent data truncation scenario.
5. Race Condition in Bronze Currency Check: Simulate a high-concurrency or multi-threaded environment where two concurrent jobs check and read stale currency data simultaneously before writing.
6. Delete Operations Unverified: Force a cascading delete or independent record deletion where the main execution reports success but dependent/foreign records are left orphaned in the database.
7. Lock Timeout Not Handled: Simulate database/row contention where a resource lock remains active, forcing the job into an indefinite retry loop without timing out gracefully.

Please use standard mock libraries and C#/.NET structures (or the relevant tech stack used in our code) to demonstrate these failures clearly.
