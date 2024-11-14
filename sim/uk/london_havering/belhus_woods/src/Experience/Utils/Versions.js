import { VERSIONS } from "../../versions"


export default class Versions
{
    constructor()
    {
        this.create_ui()
    }

    create_ui()
    {
        const body_el = document.body
        const container_el = document.createElement("div")
        body_el.appendChild(container_el)
        container_el.className = "versions"

        // Style
        const style_el = document.createElement("style")
        body_el.appendChild(style_el)
        style_el.id = "versions_style"
        style_el.innerHTML = `
            .versions {
                position: fixed;
                bottom: 0;
                right: 0;
                background-color: rgba(0, 0, 0, 0.5);
                color: white;
                font-family: sans-serif;
                font-size: 10px;
            }
        `
        // Content
        const version_el = document.createElement("div")
        container_el.appendChild(version_el)
        version_el.id = "version"

        const latest_version = VERSIONS[0]
        version_el.innerHTML = `
            <div>Version ${latest_version.version} (${latest_version.datetime})</div>
        `

        // Validate versions
        const error = validate_versions(VERSIONS)
        if (error)
        {
            style_el.innerHTML += `
            .versions {
                background-color: rgba(200, 50, 0, 0.5);
            }
            `

            version_el.title = error.toString()
            new bootstrap.Tooltip(version_el, { placement: "left"})
        }
    }
}


function validate_versions(versions)
{
    try
    {
        _validate_versions(versions)
    }
    catch(error)
    {
        console.error("Versions error:", error)
        return error
    }
}


function _validate_versions(versions)
{
    let latest_version = null

    versions.forEach(version =>
    {
        if(!version.version)
        {
            throw new Error("Version missing version number")
        }
        if(!version.datetime)
        {
            throw new Error("Version missing datetime")
        }
        if(!version.description)
        {
            throw new Error("Version missing description")
        }

        if(!latest_version)
        {
            latest_version = version
            return
        }

        const latest_semver = extract_semver(latest_version.version)
        const semver = extract_semver(version.version)
        const error_versions_not_in_order = `Versions are not in order. Version ${latest_version.version} should be before version ${version.version}`
        if(semver.major > latest_semver.major)
        {
            throw error_versions_not_in_order
        }
        else if(semver.minor > latest_semver.minor)
        {
            throw error_versions_not_in_order
        }
        else if(semver.patch >= latest_semver.patch)
        {
            throw error_versions_not_in_order
        }

        latest_version = version
    })
}

function extract_semver(version)
{
    const semver = version.match(/(\d+)\.(\d+)\.(\d+)/)
    if(!semver)
    {
        throw new Error(`Invalid version number: ${version}`)
    }
    return {
        major: parseInt(semver[1]),
        minor: parseInt(semver[2]),
        patch: parseInt(semver[3]),
    }
}
