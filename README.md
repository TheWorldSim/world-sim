# World Sim

Simulations about our world based on objective evidence.

Should be:
* transparent
* open to constructive, productive, concise criticism and improvement based on objective reproducible evidence

Should inspire trust and give us multiple options

Where there is uncertainty, represent the full diversity of scenarios.  Then give users the tools to filter by "merit", "likelihood", "impact".  Default should show all filtered by product of these attributes.

## Data

The data is not only numbers, and trends but also relationships between values.

The data lives in the [data repo](https://github.com/TheWorldSim/world-sim-data).  Though some of it lives in this code repo at the moment.


## Simulations

For now all the simulations will live in this repo.

* [Energy Explorer](https://theworldsim.org/sims/energy-explorer/index.html) - explore the potential and limits of renewable energy.


# Change log

## 2020-02-17
[Energy Explorer v0.0.1](https://theworldsim.org/sims/energy-explorer/v0.0.1/index.html) - first version

# Dev

The World Sim is still a prototype and is in alpha.  If you're a researcher in academia, industry, government, NGO or citizen scientist and have data to add or a simulation you'd like to build then get in touch.

For data please open an [issue here](https://github.com/TheWorldSim/world-sim-data/issues).  For simulation related questions or ideas please open an [issue here](https://github.com/TheWorldSim/world-sim/issues).  Alternatively please [contact us](https://theworldsim.org/#help-us-do).

## TODO

Move all data into data repo.

## Known bugs
### Outstanding - medium

### Outstanding - minor

### Dealt with via a hack

* The camera near clipping plane has to be set at the start as it forgets 0.01f and sets itself to 0.1f
* The calculation of nearest zoom doesn't work well with tilt... you seem to be able to scroll in indefinitely.

## running local builds

python -m http.server 8000
