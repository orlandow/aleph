namespace Aleph.Web.Models

type ImageId = string

type Card = {
    title: string
    image: ImageId option
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