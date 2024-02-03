using System.ComponentModel.DataAnnotations;

namespace Yakify.Api.Models;

public class MaxAttribute(double max): RangeAttribute(double.NegativeInfinity, max);