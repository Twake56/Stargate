export interface Person {
  personId: number;
  name: string;
  currentRank: string;
  currentDutyTitle: string;
  careerStartDate: Date;
  careerEndDate: Date;
}

export interface AstronautDuty {
  id: number;
  personId: number;
  rank: string;
  dutyTitle: string;
  dutyStartDate: Date;
  dutyEndDate: Date;
}

export interface PersonDutyResponse {
  astronautDuties: AstronautDuty[];
  person: Person;
  success: boolean;
  responseCode: number;
  message: string;
}
