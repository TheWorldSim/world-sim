# World Sim

Data driven simulations

Should be:
* transparent
* open to constructive, productive, concise criticism and improvement based on objective reproducible evidence

Should inspire trust and give us multiple options

Where there is uncertainty, represent the full diversity of scenarios.  Give users the tools to filter by "merit", "likelihood", "impact".  Default should show all filtered by product of these attributes.


# Change log

## 2020-02-17 v0.0.1-alpha
First version

# Dev

## Known bugs
### Outstanding - medium

### Outstanding - minor

### Dealt with via a hack
The camera near clipping plane has to be set at the start as it forgets 0.01f and sets itself to 0.1f
The calculation of nearest zoom doesn't work well with tilt... you seem to be able to scroll in indefinitely.

## running local builds

python -m http.server 8000
