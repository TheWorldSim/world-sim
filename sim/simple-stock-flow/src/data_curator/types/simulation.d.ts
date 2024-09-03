declare module "simulation" {
    interface ModelConfig
    {
        primitiveFn?: (root, type) => SimulationNode[]
        timeStart?: number
        timeStep?: number
        timeLength?: number
        timeUnits?: TimeUnitsAll // todo
        timePause?: number
    }

    interface ModelVariableConfig
    {
        name: string
        value: number | string | "True" | "False"
        units?: string
        note?: string
    }

    interface ModelStockConfig
    {
        name: string
        initial?: number | string | "True" | "False"
        units?: string
        note?: string
    }


    interface Primitive
    {
        id: string
    }


    interface onPauseResArg
    {
        times: number[]
        data: object[]
        timeUnits: TimeUnitsAll
        children: undefined
        error: null
        errorPrimitive: null
        stochastic: undefined
        value: (item: Primitive) => number
        lastValue: (item: Primitive) => number
        periods: number
        resume: () => void
        setValue: (cell: Primitive | SimulationComponent, value: number) => void
    }


    export class Model
    {
        constructor (config: ModelConfig)

        Variable (config: ModelVariableConfig): SimulationComponent { }
        Stock (config: ModelStockConfig): SimulationComponent { }
        Flow(from_id: SimulationComponent | undefined, to_id: SimulationComponent | undefined, config: { name: string; note: string, rate: string }): SimulationComponent {}

        // If config.onPause is set then the simulation will pause and return
        // value of this function will be undefined.
        simulate (config: { onPause: (res: onPauseResArg) => void }): SimulationResult | undefined { }

        Link (source_component: SimulationComponent, consuming_component: SimulationComponent) { }

        findStocks(selector: (model_item: {_node: SimulationNode, model: {_graph: {}, settings: {}, p: ()=>{} } }) => void): SimulationComponent[] { }
        getId(model_id: string): SimulationComponent | null { }
    }

    export interface SimulationError
    {
        code: number
        columnNumber: number
        lineNumber: number
        message: string
    }


    type TimeUnits = "years"
    type TimeUnitsAll = "Years" | "Seconds" | TimeUnits


    type ModelAttributeNames = "name" | "Note" | "Equation" | "Units" | "MaxConstraintUsed" | "MinConstraintUsed" | "MaxConstraint" | "MinConstraint" | "ShowSlider" | "SliderMax" | "SliderMin" | "SliderStep" | "Image" | "FlipHorizontal" | "FlipVertical" | "LabelPosition"

    interface SimulationNode
    {
        attributes: Map
        parent?: SimulationNode
        children: (SimulationNode | null)[]
        id: string
        value: {}
        _primitive: { model: {} }
        source: null
        target: null
        getAttribute (attribute_name: "Units"): string
        getAttribute (attribute_name: ModelAttributeNames): any
    }

    interface SimulationComponent
    {
        _node: SimulationNode
        model: {}
        units?: string
    }

    interface SimulationResult
    {
        _data: SimulationResult_data
        _nameIdMapping: {[index: string]: string} // maps id to Variable.name
        timeUnits: TimeUnits
    }

    interface SimulationResult_data
    {
        times: number[]
        data: {[id: string]: number}[]
        timeUnits: TimeUnits
        children: {[id: string]: {data: {}, results: number[], dataMode: "float"}}
        error: null
        errorPrimitive: null
        stochastic: boolean
        periods: number
    }
}
