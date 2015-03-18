﻿namespace Aleph.Models

type Image = byte[]

type Card = {
    title: string
    image: Image option
    data: Data list
    desc: string option
}
with 
    static member fromTitle str =
        { title = str; image = None; data = []; desc = None }

and Data = Icon * string

and Icon = Nothing | Clock

type Cards = Card list