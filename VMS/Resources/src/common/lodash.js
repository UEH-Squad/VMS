import {
    cloneDeep,
    flatten
} from 'lodash';

const CloneDeep = (object) => cloneDeep(object);

const Flatten = (array) => flatten(array);

export default { CloneDeep, Flatten };