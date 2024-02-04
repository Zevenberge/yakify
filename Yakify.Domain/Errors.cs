namespace Yakify.Domain;

public static class Errors
{
    public const string ORDER_CUSTOMER_CANNOT_BE_EMPTY = "Order customer cannot be empty";
    public const string YAK_AGE_CANNOT_BE_EMPTY = "Yak age cannot be empty";
    public const string YAK_AGE_CANNOT_BE_NEGATIVE = "Yak age cannot be negative";
    public const string YAK_AGE_BEYOND_LIFE_EXPECTANCY = "Yak age cannot be above life expectancy";
    public const string YAK_NAME_CANNOT_BE_EMPTY = "Yak name cannot be empty";
    public const string YAK_SEX_CANNOT_BE_EMPTY = "Yak sex cannot be empty";
    public const string YAK_SEX_INVALID_VALUE = "Yak sex has an invalid value";
}