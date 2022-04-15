import CreateClass from "./CreateClass";

test("should create class as concat two string", () => {
    // act
    const result = CreateClass("test", "sub");

    // assert
    expect(result).toBe("test-sub");
});
