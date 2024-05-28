using System.Collections;
using System.Collections.Generic;

public class PowerResource
{
    // Uranium:
    //  2018
    //  ref: oecd-nea.org/Uranium 2018 - Resources, Production and Demand.pdf
    //   6142200 tonnes // reasonably assured and inferred <USD 130/kgU
    //   7988600 tU     // reasonably assured and inferred <USD 260/kgU
    // 1 January 2017, estimates of undiscovered resources and unconventional resources not been updated for several years
    //   7530600 tU     // undiscovered resources (prognosticated resources and speculative resources)
    //   28500000 tU    // Unconventional resources
    //

    /*
        > As of 1 January 2017, a total of 449 commercial nuclear reactors were
        connected to the grid globally, with a net generating capacity of 391 GWe
        requiring about 62 825 tU annually.
        ref: oecd-nea.org/Uranium 2018 - Resources, Production and Demand.pdf
        This means 161 tonnes of uranium per GWe
        This agrees with `david-mackay/sewtha.pdf`

        > A once-through one-gigawatt nuclear power station uses 162 tons per year of uranium
        ref: david-mackay/sewtha.pdf
        Would give 194.7 x 10^9 J per kg one-through uranium (4.95% U235 enriched? -> "with standard 4.95 percent enriched uranium-235 fuel" ref: https://en.wikipedia.org/wiki/NuScale_Power)
        calc: ((10^9) * 365 * 24 * 3600) / 162000

        And 1kwh == 3,600,000J
        so (194.7 x 10^9 J per kg once-through-uranium-4.95%-U235 / 3,600,000J) == 54083 kwh per kg once-through-uranium-4.95%-U235
    */
}

/*
    Hydro
    UK
    670 km^2 between 800 and 1344 m
    20,000 km^2 between 400 and 800 m
    40,000 km^2 between 200 and 400 m
    63,000 km^2 between 100 and 200 m
    72,000 km^2 between  50 and 100 m
    50,000 km^2 between   0 and  50 m
    ref: http://www.withouthotair.com/c8/page_55.shtml
 */