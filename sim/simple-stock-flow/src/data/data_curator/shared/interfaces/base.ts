// Copied from: https://github.com/AJamesPhillips/DataCurator/blob/main/app/frontend/src/shared/interfaces/base.ts

import type { Color } from "./color"


export interface HasBaseId
{
    base_id: number
}

export interface Base extends HasBaseId
{
    id: string
    created_at: Date
    custom_created_at?: Date
    modified_at?: Date
    modified_by_username?: string
    deleted_at?: Date

    label_ids?: string[]
    label_color?: Color
    summary_image?: string

    // meta sync fields
    needs_save?: boolean
    saving?: boolean
}
