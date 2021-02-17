import React, { Component } from 'react';
import { Row, Col } from 'reactstrap';

export class PhotographerSearchResults extends Component {
    render() {
        return (
            <Row className="top-margin-spacing">
                <Col>
                    <table className="table table-bordered table-sm">
                        <thead className="thead-light">
                            <tr>
                                <th>First Name</th>
                                <th>Last Name</th>
                                <th>Competition #</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {this.props.photographers
                                .filter(p => p.firstName.includes(this.props.searchText) || p.lastName.includes(this.props.searchText)) // TODO: don't think this is working correctly.. debug
                                .map(result =>
                                    <tr key={result.id}>
                                        <td>{result.firstName}</td>
                                        <td>{result.lastName}</td>
                                        <td>{result.competitionNumber}</td>
                                        <td><button className="btn btn-link" onClick={(e) => { e.preventDefault(); this.props.selectPhotographer(result.id); }}>Select</button></td>
                                    </tr>
                                )}
                        </tbody>
                    </table>
                </Col>
            </Row>
        );
    }
}