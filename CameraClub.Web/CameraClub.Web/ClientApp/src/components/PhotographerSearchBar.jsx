import React, { Component } from 'react';
import { Row, Col } from 'reactstrap';
import { InputWithChanges } from './InputWithChanges';

export class PhotographerSearchBar extends Component {
    constructor(props) {
        super(props);

        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(searchValue) {
        this.props.updateSearch(searchValue.search);
    }

    render() {
        return (
            <Row>
                <Col sm={2}>
                    Name
                    </Col>
                <Col sm={10}>
                    <InputWithChanges inputType="text" name="search" placeholder="First or Last name of photographer" onChangeInput={this.handleChange} />
                </Col>
            </Row>
        );
    }
}