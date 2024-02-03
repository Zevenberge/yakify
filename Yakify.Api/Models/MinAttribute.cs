using System.ComponentModel.DataAnnotations;

namespace Yakify.Api.Models;

public class MinAttribute(double min): RangeAttribute(min, double.PositiveInfinity);
