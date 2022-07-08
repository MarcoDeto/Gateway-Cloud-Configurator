export class Entity {
    constructor(
        public name: string,
        public description: string
    ) {}
}

export class TypeEntity {
    constructor(
        public id: string,
        public description: string,
        public entity: Entity,
        public param1: string,
        public param2: string,
        public param3: string,
        public allowEditChannel: boolean,
        public allowEditVariable: boolean
    ) {}
}

export class SelectTypeEntity {
    constructor(
        public id: string,
        public description: string,
    ) {}
}