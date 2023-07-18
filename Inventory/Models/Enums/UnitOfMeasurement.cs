using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.Enums
{
    public enum UnitOfMeasurement : int
    {
        und = 0,
        m = 1,
        cm = 2,
        kg = 3,
        g = 4,
        mg = 5,
        l = 6,
        ml = 7,
        gl = 8,

        [Display(Name ="m²")]
        m2 = 9,
        
        [Display(Name = "m³")]
        m3 = 10,
        cx = 11,
        pc = 12,
        cj = 13,
        kit = 14,
        jg = 15,
        rol = 16,
        pct = 17,
        par = 18,
        lat = 19,
        tb = 20,
        br = 21,
        bd = 22,
        sc = 23,
        res = 24,
        fd = 25,
    }
}
