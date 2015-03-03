module People

type Lifespan = 
    | Alive of birth:Date
    | Dead of birth:Date * death:Date

type Profession = string

type Person = {
    name: string
    fullName: string
    lifespan: Lifespan
    country: Country
    profession: Profession
}