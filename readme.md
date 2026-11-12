
SELECT *
FROM Groups
WHERE GroupNumber = '8379';

SELECT *
FROM GroupSkipPeriods
WHERE GroupNumber = '8379';

SELECT *
FROM GroupCashDivisionTypes
WHERE GroupNumber = '8379';

DECLARE @GroupNumber VARCHAR(20) = '8379';

SELECT
    SP.GroupNumber,
    SP.StartMonth,
    SP.StartDay,
    SP.EndMonth,
    SP.EndDay,
    SP.CashDivisionTypeId,
    GCD.BillFrequencyId,
    BFM.BillFrequencyName
FROM GroupSkipPeriods SP
LEFT JOIN GroupCashDivisionTypes GCD
    ON GCD.GroupNumber = SP.GroupNumber
    AND GCD.CashDivisionTypeId = SP.CashDivisionTypeId
LEFT JOIN BillFrequencyMaster BFM






==========
public async Task<BillModesResponse?> GetBillModesAsync(
    string groupNumber,
    CancellationToken cancellationToken = default)
{
    cancellationToken.ThrowIfCancellationRequested();

    if (dbContext == null)
        throw new InvalidOperationException("Database context is not available.");

    // 1. Check group existence
    bool groupExists = await dbContext.Groups
        .AnyAsync(g => g.GroupNumber == groupNumber, cancellationToken);

    if (!groupExists)
        return null;

    // 2. Get skip periods
    var skipPeriods = await dbContext.GroupSkipPeriods
        .AsNoTracking()
        .Where(sp => sp.GroupNumber == groupNumber)
        .ToListAsync(cancellationToken);

    if (skipPeriods.Count == 0)
        return new BillModesResponse
        {
            BillModes = new List<BillModeResponse>()
        };

    // 3. Get cash division types
    var cashDivisions = await dbContext.GroupCashDivisionTypes
        .AsNoTracking()
        .Where(gcd => gcd.GroupNumber == groupNumber)
        .ToListAsync(cancellationToken);

    // 4. Get bill frequency names
    var frequencyNames =
        await LoadBillFrequencyNamesAsync(cancellationToken);

    // 5. Build bill modes
    var billModes = skipPeriods.Select(sp =>
    {
        var cashDivision = cashDivisions.FirstOrDefault(gcd =>
            gcd.CashDivisionTypeId == sp.CashDivisionTypeId);

        string? frequencyName = null;

        if (cashDivision != null)
            frequencyNames.TryGetValue(
                cashDivision.BillFrequencyId, out frequencyName);

        return new BillModeResponse
        {
            BillModeId = GenerateBillModeId(
                sp.StartMonth, sp.StartDay,
                sp.EndMonth, sp.EndDay),

            BillModeDescription = GenerateBillModeDescription(
                sp.StartMonth, sp.StartDay,
                sp.EndMonth, sp.EndDay),

            BillFrequency = frequencyName,

            SkipStartDay = sp.StartDay,
            SkipStartMonth = sp.StartMonth,
            SkipEndDay = sp.EndDay,
            SkipEndMonth = sp.EndMonth
        };
    }).ToList();

    return new BillModesResponse
    {
        BillModes = billModes.AsReadOnly()
    };
}

==============
private async Task<Dictionary<int, string>> LoadBillFrequencyNamesAsync(
    CancellationToken cancellationToken)
{
    try
    {
        return await dbContext.BillFrequencies
            .AsNoTracking()
            .ToDictionaryAsync(
                bf => bf.BillFrequencyId,
                bf => bf.BillFrequencyName,
                cancellationToken);
    }
    catch
    {
        return new Dictionary<int, string>();
    }
}




    ON BFM.BillFrequencyId = GCD.BillFrequencyId
WHERE SP.GroupNumber = @GroupNumber;
