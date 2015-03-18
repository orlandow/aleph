namespace Aleph.Web.Models

type Image = byte[]

type Card = {
    title: string
    image: Image option
    data: Data list
    desc: string option
    code: string option
    aside: string option
}
with 
    static member fromTitle str =
        { title = str; image = None; data = []; desc = None; code = None; aside = None }

and Data = Icon * string

and Icon = Nothing | Clock

type Cards = Card list